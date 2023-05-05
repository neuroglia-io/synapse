namespace Synapse;

/// <summary>
/// Exposes workflow activity outcomes supported by default
/// </summary>
public static class WorkflowActivityOutcome
{

    /// <summary>
    /// Gets the 'next' workflow activity outcome, used to execute the next activity in the workflow pipeline
    /// </summary>
    public const string Next = "next";
    /// <summary>
    /// Gets the 'end' workflow activity outcome, used to end the execution of the workflow
    /// </summary>
    public const string End = "end";

} 