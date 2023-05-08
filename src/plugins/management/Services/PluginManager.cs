using Microsoft.Extensions.Hosting;
using NuGet.Common;
using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using Synapse.Plugins.Sdk;
using Synapse.Plugins.Skd;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Runtime.Versioning;
using System.Text.Json;

namespace Synapse.Plugins.Management.Services;

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

    /// <summary>
    /// Gets a <see cref="ConcurrentDictionary{TKey, TValue}"/>
    /// </summary>
    protected ConcurrentDictionary<string, PluginMetadata> AvailablePlugins { get; } = new();

    /// <inheritdoc/>
    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!this.PluginsDirectory.Exists) this.PluginsDirectory.Create();
        foreach(var pluginFile in this.PluginsDirectory.GetFiles("plugin.json", SearchOption.AllDirectories))
        {
            var json = await File.ReadAllTextAsync(pluginFile.FullName, cancellationToken).ConfigureAwait(false);
            var pluginMetadata = JsonSerializer.Deserialize<PluginMetadata>(json)!;
            if (!string.IsNullOrWhiteSpace(pluginMetadata.NugetPackage)) await this.DownloadAndExtractNugetPackageAsync(pluginMetadata.NugetPackage, cancellationToken).ConfigureAwait(false);
        }
        foreach (var assemblyFile in this.PluginsDirectory.GetFiles("*.dll", SearchOption.AllDirectories))
        {
            var assemblyFiles = new List<string>(Directory.GetFiles(RuntimeEnvironment.GetRuntimeDirectory(), "*.dll")) { assemblyFile.FullName };
            assemblyFiles.AddRange(AssemblyLoadContext.Default.Assemblies.Select(a => a.Location));
            var resolver = new PathAssemblyResolver(assemblyFiles);
            using var metadataContext = new MetadataLoadContext(resolver);
            var assembly = metadataContext.LoadFromAssemblyPath(assemblyFile.FullName);
            foreach (var type in assembly.GetTypes().Where(t => t.IsClass && !t.IsInterface && !t.IsAbstract && !t.IsGenericType))
            {

            }
        }
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<TContract> FindPluginsAsync<TContract>([EnumeratorCancellation] CancellationToken cancellationToken = default) 
        where TContract : class
    {
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
                var plugin = new Plugin(this.ServiceProvider, PluginMetadata.FromType(type));
                yield return await plugin.LoadAsync<TContract>(cancellationToken).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Downloads and extracts the specified Nuget package
    /// </summary>
    /// <param name="packageReference">A reference to the Nuget package to download and extract</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="ValueTask"/></returns>
    protected virtual async ValueTask DownloadAndExtractNugetPackageAsync(string packageReference, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(packageReference)) throw new ArgumentNullException(nameof(packageReference));
        var components = packageReference.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var packageId = components.Last();
        var packageSource = packageReference[..(packageReference.Length - packageId.Length + 1)];
        components = packageId.Split(':', StringSplitOptions.RemoveEmptyEntries);
        packageId = components[0];
        var packageVersion = components[1];
        var repository = Repository.Factory.GetCoreV3(packageSource);
        var cache = new SourceCacheContext();
        var packageSearchResource = await repository.GetResourceAsync<PackageSearchResource>().ConfigureAwait(false);
        var findPackageByIdResource = await repository.GetResourceAsync<FindPackageByIdResource>().ConfigureAwait(false);
        var searchFilter = new SearchFilter(includePrerelease: true);
        var results = await packageSearchResource.SearchAsync("", searchFilter, 0, 100, NullLogger.Instance, cancellationToken).ConfigureAwait(false);
        var result = results.FirstOrDefault() ?? throw new NullReferenceException($"Failed to find nuget package with id '{packageId}' in source '{packageSource}'");
        var versions = await result.GetVersionsAsync().ConfigureAwait(false);
        var version = versions.OrderByDescending(v => v.Version.Version).FirstOrDefault() ?? throw new NullReferenceException($"Failed to find version '{packageVersion}' of nuget package '{packageId}' in source '{packageSource}'");
        var packageFileName = Path.Combine(this.PluginsDirectory.FullName, $"{result.Identity.Id}.{version.Version}.nupkg");
        var packageOutputDirectory = Path.Combine(this.PluginsDirectory.FullName, result.Identity.Id);
        Stream packageStream;
        if (File.Exists(packageFileName))
        { 
            packageStream = File.OpenRead(packageFileName);
        }
        else
        {
            packageStream = File.OpenWrite(packageFileName);
            await findPackageByIdResource.CopyNupkgToStreamAsync(result.Identity.Id, version.Version, packageStream, cache, NullLogger.Instance, cancellationToken).ConfigureAwait(false);
            await packageStream.FlushAsync(cancellationToken).ConfigureAwait(false);
            packageStream.Position = 0;
        }
        using var packageReader = new PackageArchiveReader(packageStream);
        var currentFramework = NuGetFramework.Parse(Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName);
        var framework = (await packageReader.GetLibItemsAsync(cancellationToken).ConfigureAwait(false))
            .Where(f => DefaultCompatibilityProvider.Instance.IsCompatible(currentFramework, f.TargetFramework))
            .Last();
        foreach (var item in framework.Items)
        {
            var outputFile = Path.Combine(packageOutputDirectory, item);
            packageReader.ExtractFile(item, outputFile, NullLogger.Instance);
        }
        await packageStream.DisposeAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

}
