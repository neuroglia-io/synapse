using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Synapse.Plugins.Sdk;
using Synapse.Plugins.Sdk.Services;
using Synapse.Plugins.Skd;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace Synapse.Plugins.Management.Services;

/// <summary>
/// Defines the fundamentals of a service used to manage <see cref="IPlugin"/>s
/// </summary>
public interface IPluginManager
{

    /// <summary>
    /// Scans the specified directory for all plugins that define the specified contract
    /// </summary>
    /// <typeparam name="TContract">The type of the contract to get plugin implementations for</typeparam>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IAsyncEnumerable{T}"/>, used to asynchronously enumerate matching plugings</returns>
    IAsyncEnumerable<TContract> FindPluginsAsync<TContract>(CancellationToken cancellationToken = default)
        where TContract : class;

}

/// <summary>
/// Defines the fundamentals of a service used to describe and manage an <see cref="IPlugin"/>
/// </summary>
public interface IPlugin
    : IDisposable, IAsyncDisposable
{

    /// <summary>
    /// Gets an object that describes the <see cref="IPlugin"/>
    /// </summary>
    PluginMetadata Metadata { get; }

    /// <summary>
    /// Loads the <see cref="IPlugin"/>
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The loaded plugin</returns>
    Task<object> LoadAsync(CancellationToken cancellationToken = default);

}

/// <summary>
/// Represents the default implementation of the <see cref="IPluginManager"/> interface
/// </summary>
public class PluginManager
    : IHostedService, IPluginManager
{

    /// <summary>
    /// Initializes a new <see cref="PluginManager"/>
    /// </summary>
    /// <param name="serviceProvider">The current <see cref="IServiceProvider"/></param>
    public PluginManager(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets the <see cref="DirectoryInfo"/> to scan for plugins
    /// </summary>
    protected DirectoryInfo PluginsDirectory { get; } = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, "plugins"));

    /// <inheritdoc/>
    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<TContract> FindPluginsAsync<TContract>([EnumeratorCancellation] CancellationToken cancellationToken = default) 
        where TContract : class
    {
        if (!this.PluginsDirectory.Exists) this.PluginsDirectory.Create();
        foreach (var assemblyFile in this.PluginsDirectory.GetFiles("*.dll", SearchOption.AllDirectories))
        {
            var assemblyFiles = new List<string>(Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll")) { assemblyFile.FullName };
            assemblyFiles.AddRange(AssemblyLoadContext.Default.Assemblies.Select(a => a.Location));
            var resolver = new PathAssemblyResolver(assemblyFiles);
            using var metadataContext = new MetadataLoadContext(resolver);
            var assembly = metadataContext.LoadFromAssemblyPath(assemblyFile.FullName);
            foreach (var type in assembly.GetTypes().Where(t => t.IsClass && !t.IsInterface && !t.IsAbstract && !t.IsGenericType))
            {
                var pluginAttribute = type.GetCustomAttributesData().FirstOrDefault(a => a.AttributeType.FullName == typeof(PluginAttribute).FullName);
                if (pluginAttribute == null) continue;
                var plugin = new Plugin(this.ServiceProvider, new(type));
                yield return await plugin.LoadAsync<TContract>(cancellationToken).ConfigureAwait(false);
            }
        }
    }

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

}

/// <summary>
/// Represents the default implementation of the <see cref="IPlugin"/> interface
/// </summary>
public class Plugin
    : IPlugin
{

    bool _disposed;

    /// <summary>
    /// Initializes a new <see cref="Plugin"/>
    /// </summary>
    /// <param name="hostServices">The host's <see cref="IServiceProvider"/></param>
    /// <param name="metadata">An object that described the <see cref="IPlugin"/></param>
    public Plugin(IServiceProvider hostServices, PluginMetadata metadata)
    {
        this.HostServices = hostServices;
        this.Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
    }

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>
    /// </summary>
    protected IServiceProvider HostServices { get; }

    /// <inheritdoc/>
    public PluginMetadata Metadata { get; }

    /// <summary>
    /// Gets the <see cref="IPlugin"/>'s <see cref="ServiceProvider"/>
    /// </summary>
    protected ServiceProvider? PluginServices { get; private set; }

    /// <summary>
    /// Gets the <see cref="IPlugin"/>'s <see cref="AssemblyLoadContext"/>
    /// </summary>
    protected AssemblyLoadContext? LoadContext { get; set; }

    /// <summary>
    /// Gets the <see cref="IPlugin"/>'s <see cref="System.Reflection.Assembly"/>
    /// </summary>
    protected Assembly? Assembly { get; set; }

    /// <summary>
    /// Gets the <see cref="Plugin"/>'s <see cref="System.Threading.CancellationTokenSource"/>
    /// </summary>
    protected CancellationTokenSource CancellationTokenSource { get; private set; } = null!;

    /// <summary>
    /// Gets the loaded plugin instance, if any
    /// </summary>
    protected object? Instance { get; private set; }

    /// <inheritdoc/>
    public async Task<object> LoadAsync(CancellationToken cancellationToken = default)
    {
        if (this.Instance != null) return this.Instance;
        try
        {
            this.CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            this.LoadContext = new PluginAssemblyLoadContext(this.Metadata.AssemblyFilePath);
            this.Assembly = this.LoadContext.LoadFromAssemblyName(new(Path.GetFileNameWithoutExtension(this.Metadata.AssemblyFilePath)));
            var pluginType = this.Assembly.GetType(this.Metadata.TypeName, true)!;
            var services = new ServiceCollection();
            Type? bootstrapperType = null;
            if (this.Metadata.BootstrapperTypeName != null) bootstrapperType = this.Assembly.GetType(this.Metadata.BootstrapperTypeName);
            if(bootstrapperType != null)
            {
                var bootstrapper = (IPluginBootstrapper)ActivatorUtilities.CreateInstance(this.HostServices, bootstrapperType);
                bootstrapper.ConfigureServices(services);
            }
            this.PluginServices = services.BuildServiceProvider();
            this.Instance = ActivatorUtilities.CreateInstance(this.PluginServices, pluginType);
            return this.Instance;
        }
        catch
        {
            await this.DisposeAsync().ConfigureAwait(false);
            throw;
        }
    }

    /// <summary>
    /// Disposes of the <see cref="Plugin"/>
    /// </summary>
    /// <param name="disposing">A boolean indicating whether or not the <see cref="Plugin"/> is being disposed of</param>
    protected virtual async ValueTask DisposeAsync(bool disposing)
    {
        if (this._disposed || this._disposed) return;
        if (this.PluginServices != null) await this.PluginServices.DisposeAsync().ConfigureAwait(false);
        this.LoadContext?.Unload();
        this.CancellationTokenSource?.Dispose();
        this._disposed = true;
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await this.DisposeAsync(true).ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the <see cref="Plugin"/>
    /// </summary>
    /// <param name="disposing">A boolean indicating whether or not the <see cref="Plugin"/> is being disposed of</param>
    protected virtual void Dispose(bool disposing)
    {
        if (this._disposed || this._disposed) return;
        this.PluginServices?.Dispose();
        this.LoadContext?.Unload();
        this.CancellationTokenSource?.Dispose();
        this._disposed = true;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

}
