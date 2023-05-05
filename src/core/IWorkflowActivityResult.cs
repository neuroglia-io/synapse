namespace Synapse;

/// <summary>
/// Defines the result of a workflow activity
/// </summary>
public interface IWorkflowActivityResult
{

    /// <summary>
    /// Gets a value that describes the outcome of the activity
    /// </summary>
    string Outcome { get; }

    /// <summary>
    /// Gets the activity's output data, if any
    /// </summary>
    object? OutputData { get; }

}