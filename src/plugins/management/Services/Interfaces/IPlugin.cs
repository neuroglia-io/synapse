using Synapse.Plugins.Skd;

namespace Synapse.Plugins.Management.Services;

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
