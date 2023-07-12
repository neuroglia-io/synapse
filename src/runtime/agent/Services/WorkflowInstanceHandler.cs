namespace Synapse.Runtime.Agent.Services;

/// <summary>
/// Represents a service used to handle a <see cref="WorkflowInstance"/>
/// </summary>
public class WorkflowInstanceHandler
    : IHostedService, IDisposable, IAsyncDisposable
{

    bool _disposed;

    /// <summary>
    /// Initializes a new <see cref="WorkflowInstanceHandler"/>
    /// </summary>
    /// <param name="repository">The current <see cref="IRepository"/></param>
    /// <param name="instance">The service used to monitor the handled <see cref="WorkflowInstance"/></param>
    public WorkflowInstanceHandler(IRepository repository, IResourceMonitor<WorkflowInstance> instance)
    {
        this.Repository = repository;
        this.Instance = instance;
    }

    /// <summary>
    /// Gets the current <see cref="IRepository"/>
    /// </summary>
    protected IRepository Repository { get; }

    /// <summary>
    /// Gets the service used to monitor the handled <see cref="WorkflowInstance"/>
    /// </summary>
    protected IResourceMonitor<WorkflowInstance> Instance { get; }

    /// <summary>
    /// Gets the <see cref="WorkflowInstanceHandler"/>'s <see cref="CancellationTokenSource"/>
    /// </summary>
    protected CancellationTokenSource CancellationTokenSource { get; private set; } = null!;

    /// <inheritdoc/>
    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        this.CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

    }

    /// <inheritdoc/>
    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        this.CancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Disposes of the <see cref="WorkflowInstanceHandler"/>
    /// </summary>
    /// <param name="disposing">A boolean indicating whether or not the <see cref="WorkflowInstanceHandler"/> is being disposed of</param>
    protected virtual ValueTask DisposeAsync(bool disposing)
    {
        if (this._disposed || !disposing) return ValueTask.CompletedTask;
        this.CancellationTokenSource?.Dispose();
        this._disposed = true;
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await this.DisposeAsync(true).ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the <see cref="WorkflowInstanceHandler"/>
    /// </summary>
    /// <param name="disposing">A boolean indicating whether or not the <see cref="WorkflowInstanceHandler"/> is being disposed of</param>
    protected virtual void Dispose(bool disposing)
    {
        if (this._disposed || !disposing) return;
        this.CancellationTokenSource?.Dispose();
        this._disposed = true;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

}
