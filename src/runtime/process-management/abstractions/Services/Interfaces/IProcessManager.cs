using Synapse.Resources;

namespace Synapse.Runtime.ProcessManagement.Services;

/// <summary>
/// Defines the fundamentals of a service used to manage <see cref="IProcess"/>es
/// </summary>
public interface IProcessManager
{

    /// <summary>
    /// Creates a new <see cref="IProcess"/>
    /// </summary>
    /// <param name="configuration">An object used to configure the <see cref="IProcess"/> to create</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IProcess"/></returns>
    Task<IProcess> CreateProcessAsync(ProcessConfiguration configuration, CancellationToken cancellationToken = default);

}