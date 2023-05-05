using Synapse.Plugins.Sdk;
using Synapse.Resources;
using System.Diagnostics;

namespace Synapse.Runtime.ProcessManagement.Services;

/// <summary>
/// Represents an Operational System reliant <see cref="IProcessManager"/> implementation
/// </summary>
[Plugin(typeof(IProcessManager))]
public class NativeProcessManager
    : IProcessManager
{

    /// <inheritdoc/>
    public virtual Task<IProcess> CreateProcessAsync(ProcessConfiguration configuration, CancellationToken cancellationToken = default)
    {
        if(configuration == null) throw new ArgumentNullException(nameof(configuration));
        var startInfo = new ProcessStartInfo()
        {
            FileName = configuration.Target,
            //WorkingDirectory = this.Options.WorkingDirectory, //todo: urgent: do something about this
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            UseShellExecute = false
        };
        configuration.Arguments?.ForEach(startInfo.ArgumentList.Add);
        var process = new Process()
        {
            StartInfo = startInfo,
            EnableRaisingEvents = true
        };
        return Task.FromResult<IProcess>(new NativeProcess(process));
    }

}