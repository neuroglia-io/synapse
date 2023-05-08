using Microsoft.Extensions.DependencyInjection;
using Synapse.Plugins.Sdk.Services;
using Synapse.Plugins.Skd;
using System.Reflection;
using System.Runtime.Loader;

namespace Synapse.Plugins.Management.Services;

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
        if(this.Instance != null)
        {
            switch(this.Instance)
            {
                case IAsyncDisposable asyncDisposable: await asyncDisposable.DisposeAsync().ConfigureAwait(false); break;
                case IDisposable disposable: disposable.Dispose(); break;
            }
        }
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
        if (this.Instance != null && this.Instance is IDisposable disposable) disposable.Dispose();
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
