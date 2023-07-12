using Synapse.Plugins.Services;

namespace Synapse.Plugins;

/// <summary>
/// Defines extensions for <see cref="IPluginManager"/>s
/// </summary>
public static class IPluginManagerExtensions
{

    /// <summary>
    /// Finds the first <see cref="IPlugin"/> that implements the specified contract type
    /// </summary>
    /// <typeparam name="TContract">The type of the <see cref="IPlugin"/>'s contract</typeparam>
    /// <param name="pluginManager">The extended <see cref="IPluginManager"/></param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The loaded <see cref="IPlugin"/></returns>
    public static async Task<TContract?> FindPluginAsync<TContract>(this IPluginManager pluginManager, CancellationToken cancellationToken = default)
        where TContract : class
    {
        return await pluginManager.FindPluginsAsync<TContract>(cancellationToken).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

}