using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Synapse.Plugins.Services;
using Synapse.Resources;

namespace Synapse.Runtime.ProcessManagement.Services;

/// <summary>
/// Represents the default, plugin-based implementation of the <see cref="IProcessManager"/>
/// </summary>
public class PluginProcessManager
    : IHostedService, IProcessManager
{

    /// <summary>
    /// Initializes a new <see cref="PluginProcessManager"/>
    /// </summary>
    /// <param name="loggerFactory">The service used to create <see cref="ILogger"/>s</param>
    /// <param name="pluginManager">The service used to manage plugins</param>
    public PluginProcessManager(ILoggerFactory loggerFactory, IPluginManager pluginManager)
    {
        this.Logger = loggerFactory.CreateLogger(this.GetType());
        this.PluginManager = pluginManager;
    }

    /// <summary>
    /// Gets the service used to perform logging
    /// </summary>
    protected ILogger Logger { get; }

    /// <summary>
    /// Gets the service used to manage plugins
    /// </summary>
    protected IPluginManager PluginManager { get; }

    /// <summary>
    /// Gets the <see cref="IProcessManager"/> plugin
    /// </summary>
    protected IProcessManager Plugin { get; private set; } = null!;

    /// <summary>
    /// Gets the <see cref="PluginProcessManager"/>'s <see cref="System.Threading.CancellationTokenSource"/>
    /// </summary>
    protected CancellationTokenSource CancellationTokenSource { get; private set; } = null!;

    /// <inheritdoc/>
    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        var pluginsDirectory = Path.Combine(AppContext.BaseDirectory, "plugins");
        var plugin = await this.PluginManager.FindPluginsAsync<IProcessManager>(cancellationToken).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        if (plugin == null)
        {
            this.Logger.LogError("Failed to find a workflow process manager plugin");
            throw new NullReferenceException($"Failed to find a valid workflow process manager plugin, required when using the '{nameof(PluginProcessManager)}'");
        }
        this.Plugin = plugin;
    }

    /// <inheritdoc/>
    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        this.CancellationTokenSource?.Cancel();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual Task<IProcess> CreateProcessAsync(ProcessConfiguration configuration, CancellationToken cancellationToken = default)
    {
        return this.Plugin.CreateProcessAsync(configuration, cancellationToken);
    }

}
