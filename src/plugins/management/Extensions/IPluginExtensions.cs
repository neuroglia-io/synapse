using Synapse.Plugins.Management.Services;

namespace Synapse.Plugins.Management;

/// <summary>
/// Defines extensions for <see cref="IPlugin"/>s
/// </summary>
public static class IPluginExtensions
{

    /// <summary>
    /// Loads the <see cref="IPlugin"/>
    /// </summary>
    /// <typeparam name="TContract">The type of the <see cref="IPlugin"/>'s contract</typeparam>
    /// <param name="plugin">The extended <see cref="IPlugin"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The loaded <see cref="IPlugin"/></returns>
    public static async Task<TContract> LoadAsync<TContract>(this IPlugin plugin, CancellationToken cancellationToken = default)
        where TContract : class
    {
        return (TContract)await plugin.LoadAsync(cancellationToken).ConfigureAwait(false);
    }

}
