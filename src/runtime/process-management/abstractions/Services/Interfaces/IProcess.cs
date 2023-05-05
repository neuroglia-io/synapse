namespace Synapse.Runtime.ProcessManagement.Services;

/// <summary>
/// Defines the fundamentals of a process
/// </summary>
public interface IProcess
    : IDisposable, IAsyncDisposable
{

    /// <summary>
    /// The event fired whenever the <see cref="IProcess"/> exits
    /// </summary>
    event EventHandler? Exited;

    /// <summary>
    /// Gets the <see cref="IProcess"/>'s id
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Gets a value describing the <see cref="IProcess"/>'s phase
    /// </summary>
    ProcessPhase Phase { get; }

    /// <summary>
    /// Gets the <see cref="IProcess"/>'s exit code
    /// </summary>
    long? ExitCode { get; }

    /// <summary>
    /// Starts the <see cref="IProcess"/>
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="ValueTask"/></returns>
    ValueTask StartAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Terminates the <see cref="IProcess"/>
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="ValueTask"/></returns>
    ValueTask StopAsync(CancellationToken cancellationToken = default);

}
