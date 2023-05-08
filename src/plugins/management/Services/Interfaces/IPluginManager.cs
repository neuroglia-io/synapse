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
