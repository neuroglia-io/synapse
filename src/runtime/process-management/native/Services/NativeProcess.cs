using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;

namespace Synapse.Runtime.ProcessManagement.Services;

/// <summary>
/// Represents an Operational System reliant <see cref="IProcess"/> implementation
/// </summary>
public class NativeProcess
    : IProcess
{

    /// <inheritdoc/>
    public event EventHandler? Exited;

    bool _disposed;
    bool _started;

    /// <summary>
    /// Initializes a new <see cref="NativeProcess"/>
    /// </summary>
    /// <param name="process">The underlying <see cref="System.Diagnostics.Process"/></param>
    public NativeProcess(Process process)
    {
        this.Process = process ?? throw new ArgumentNullException(nameof(process));
    }

    /// <inheritdoc/>
    public string Id => this.Process.Id.ToString();

    /// <inheritdoc/>
    public ProcessPhase Phase => this._started ? this.Process.HasExited ? ProcessPhase.Exited : !this.Process.Responding ? ProcessPhase.NotResponding : ProcessPhase.Running : ProcessPhase.Pending;

    /// <inheritdoc/>
    public long? ExitCode => this.Process.HasExited ? this.Process.ExitCode : null;

    /// <summary>
    /// Gets the underlying <see cref="System.Diagnostics.Process"/>
    /// </summary>
    protected Process Process { get; }

    /// <summary>
    /// Gets the service used to write the <see cref="NativeProcess"/>'s logs
    /// </summary>
    public virtual StringBuilder LogsWriter { get; } = new();

    /// <summary>
    /// Gets the <see cref="NativeProcess"/>'s <see cref="System.Threading.CancellationTokenSource"/>
    /// </summary>
    protected CancellationTokenSource CancellationTokenSource { get; private set; } = null!;

    /// <inheritdoc/>
    public virtual ValueTask StartAsync(CancellationToken cancellationToken = default)
    {
        this.CancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        this.Process.GetLogsAsObservable().Subscribe(l => this.LogsWriter.AppendLine(l), this.CancellationTokenSource.Token);
        this.Process.Start();
        this.Process.BeginOutputReadLine();
        this.Process.BeginErrorReadLine();
        this._started = true;
        return ValueTask.CompletedTask;
    }

    /// <inheritdoc/>
    public virtual ValueTask StopAsync(CancellationToken cancellationToken = default)
    {
        this.Process.Close();
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Handles the <see cref="NativeProcess"/>'s having exited
    /// </summary>
    protected virtual void OnExited()
    {
        this.Exited?.Invoke(this, new());
    }

    /// <summary>
    /// Disposes of the <see cref="NativeProcess"/>
    /// </summary>
    /// <param name="disposing">A boolean indicating whether or not the <see cref="NativeProcess"/> is being disposed of</param>
    protected virtual ValueTask DisposeAsync(bool disposing)
    {
        if (this._disposed || this._disposed) return ValueTask.CompletedTask;

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
    /// Disposes of the <see cref="NativeProcess"/>
    /// </summary>
    /// <param name="disposing">A boolean indicating whether or not the <see cref="NativeProcess"/> is being disposed of</param>
    protected virtual void Dispose(bool disposing)
    {
        if (this._disposed || this._disposed) return;

        this._disposed = true;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

}