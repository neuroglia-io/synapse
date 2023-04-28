using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;

namespace Synapse.Runner.Services;

/// <summary>
/// Represents a service used to observe, run and manage <see cref="WorkflowProcess"/>es
/// </summary>
public class WorkflowProcessManager
    : BackgroundService, IAsyncDisposable
{

    bool _disposed;

    /// <summary>
    /// Initializes a new <see cref="WorkflowProcessManager"/>
    /// </summary>
    /// <param name="serviceProvider">The current <see cref="IServiceProvider"/></param>
    /// <param name="repository">The current <see cref="IRepository"/></param>
    public WorkflowProcessManager(IServiceProvider serviceProvider, IRepository repository)
    {
        this.ServiceProvider = serviceProvider;
        this.Repository = repository;
    }

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets the current <see cref="IRepository"/>
    /// </summary>
    protected IRepository Repository { get; }

    /// <summary>
    /// Gets the service used to monitor the <see cref="WorkflowRunner"/> resource
    /// </summary>
    protected IResourceMonitor<WorkflowRunner> Configuration { get; private set; } = null!;

    /// <summary>
    /// Gets the <see cref="IResourceWatch"/> used to observe <see cref="WorkflowProcess"/>es
    /// </summary>
    protected IResourceWatch<WorkflowProcess> Processes { get; private set; } = null!;

    /// <summary>
    /// Gets a <see cref="ConcurrentDictionary{TKey, TValue}"/> used to keep track of managed <see cref="WorkflowProcessHandler"/>s
    /// </summary>
    protected ConcurrentDictionary<string, WorkflowProcessHandler> ProcessHandlers { get; } = new();

    /// <summary>
    /// Gets the <see cref="WorkflowProcessManager"/>'s <see cref="System.Threading.CancellationTokenSource"/>
    /// </summary>
    protected CancellationTokenSource CancellationTokenSource { get; private set; } = null!;

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
        this.Configuration = await this.Repository.MonitorAsync<WorkflowRunner>(Environment.GetEnvironmentVariable("SYNAPSE_RUNNER_NAME")!, Environment.GetEnvironmentVariable("SYNAPSE_RUNNER_NAMESPACE")!, this.CancellationTokenSource.Token).ConfigureAwait(false); //todo: urgent: configuration-based
        var labelSelectors = new List<LabelSelector>() { new LabelSelector(WorkflowDsl.Labels.Reference, LabelSelectionOperator.Contains, string.Join(',', this.Configuration.Resource.Spec.SupportedLanguages)) };
        this.Processes = await this.Repository.WatchAsync<WorkflowProcess>(null, labelSelectors, this.CancellationTokenSource.Token).ConfigureAwait(false);
        this.Processes.Where(e => e.Type == ResourceWatchEventType.Created).Select(e => e.Resource).SubscribeAsync(OnProcessCreatedAsync, cancellationToken: this.CancellationTokenSource.Token);
        this.Processes.Where(e => e.Type == ResourceWatchEventType.Deleted).Select(e => e.Resource).SubscribeAsync(OnProcessDeletedAsync, cancellationToken: this.CancellationTokenSource.Token);
    }

    /// <summary>
    /// Handles the creation of a new <see cref="WorkflowProcess"/>
    /// </summary>
    /// <param name="process">The newly created <see cref="WorkflowProcess"/></param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    protected virtual async Task OnProcessCreatedAsync(WorkflowProcess process)
    {
        var monitor = new ResourceMonitor<WorkflowProcess>(this.Processes, process);
        var handler = (WorkflowProcessHandler)ActivatorUtilities.CreateInstance(this.ServiceProvider, typeof(WorkflowProcessHandler), this.Processes);
        this.ProcessHandlers.AddOrUpdate(process.Spec.Key, handler, (key, current) =>
        {
            current.Dispose();
            return handler;
        });
        await handler.StartAsync(this.CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    /// Handles the deletion of an existing <see cref="WorkflowProcess"/>
    /// </summary>
    /// <param name="process">The deleted <see cref="WorkflowProcess"/></param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    protected virtual async Task OnProcessDeletedAsync(WorkflowProcess process)
    {
        if (!this.ProcessHandlers.Remove(process.Spec.Key, out var handler)) return;
        await handler.StopAsync(this.CancellationTokenSource.Token).ConfigureAwait(false);
        await handler.DisposeAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Disposes of the <see cref="WorkflowProcessHandler"/>
    /// </summary>
    /// <param name="disposing">A boolean indicating whether or not the <see cref="WorkflowProcessHandler"/> is being disposed of</param>
    protected virtual async ValueTask DisposeAsync(bool disposing)
    {
        if (this._disposed || !disposing) return;
        this.CancellationTokenSource?.Dispose();
        if(this.Configuration != null) await this.Configuration.DisposeAsync().ConfigureAwait(false);
        if(this.Processes != null) await this.Processes.DisposeAsync().ConfigureAwait(false);
        this._disposed = true;
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await this.DisposeAsync(true).ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the <see cref="WorkflowProcessHandler"/>
    /// </summary>
    /// <param name="disposing">A boolean indicating whether or not the <see cref="WorkflowProcessHandler"/> is being disposed of</param>
    protected virtual void Dispose(bool disposing)
    {
        if (this._disposed || !disposing) return;
        this.CancellationTokenSource?.Dispose();
        this.Configuration?.Dispose();
        this.Processes?.Dispose();
        this._disposed = true;
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

}
