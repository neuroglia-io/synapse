using System.Diagnostics;
using System.Reactive.Linq;

namespace Synapse.Runtime.ProcessManagement;

/// <summary>
/// Defines extensions for <see cref="Process"/>es
/// </summary>
public static class ProcessExtensions
{

    /// <summary>
    /// Gets a new <see cref="IObservable{T}"/> to observe the <see cref="Process"/>'s logs
    /// </summary>
    /// <param name="process">The <see cref="Process"/> to observe the logs of</param>
    /// <returns>A new <see cref="IObservable{T}"/> used to observe the process's logs</returns>
    public static IObservable<string> GetLogsAsObservable(this Process process)
    {
        var output = Observable.FromEventPattern<DataReceivedEventHandler, DataReceivedEventArgs>(handler => process.OutputDataReceived += handler, handler => process.OutputDataReceived -= handler);
        var error = Observable.FromEventPattern<DataReceivedEventHandler, DataReceivedEventArgs>(handler => process.ErrorDataReceived += handler, handler => process.ErrorDataReceived -= handler);
        var logs = Observable.Merge(output, error).Where(l => !string.IsNullOrWhiteSpace(l?.EventArgs?.Data)).Select(l => l.EventArgs.Data!);
        return logs;
    }

    /// <summary>
    /// Gets a new <see cref="IAsyncEnumerable{T}"/> to enumerate the <see cref="Process"/>'s logs
    /// </summary>
    /// <param name="process">The <see cref="Process"/> to enumerate the logs of</param>
    /// <returns>A new <see cref="IAsyncEnumerable{T}"/> used to enumerate the process's logs</returns>
    public static IAsyncEnumerable<string> GetLogsAsync(this Process process) => process.GetLogsAsObservable().ToAsyncEnumerable();

}