namespace Synapse.Resources;

/// <summary>
/// Represents an object used to describe the result of a workflow activity
/// </summary>
[DataContract]
public record WorkflowActivityResult
    : IExtensible
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowActivityResult"/>
    /// </summary>
    public WorkflowActivityResult() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowActivityResult"/>
    /// </summary>
    /// <param name="outcome">A value that describes the outcome of the activity</param>
    /// <param name="outputData">The activity's output data, if any</param>
    /// <param name="fault">An object used to describe the fault that occured during the execution of the activity, if any</param>
    public WorkflowActivityResult(string outcome, object? outputData = null, WorkflowFault? fault = null)
    {
        if (string.IsNullOrWhiteSpace(outcome)) throw new ArgumentNullException(nameof(outcome));
        this.Outcome = outcome;
        this.OutputData = outputData;
        this.Fault = fault;
    }

    /// <summary>
    /// Gets/sets a value that describes the outcome of the activity
    /// </summary>
    [Required]
    [DataMember(Order = 1, Name = "outcome", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("outcome"), YamlMember(Order = 1, Alias = "outcome")]
    public virtual string Outcome { get; set; } = null!;

    /// <summary>
    /// Gets/sets the activity's output data, if any
    /// </summary>
    [DataMember(Order = 2, Name = "outputData"), JsonPropertyOrder(2), JsonPropertyName("outputData"), YamlMember(Order = 2, Alias = "outputData")]
    public virtual object? OutputData { get; set; }

    /// <summary>
    /// Gets/sets an object used to describe the fault that occured during the execution of the activity, if any
    /// </summary>
    [DataMember(Order = 3, Name = "fault"), JsonPropertyOrder(3), JsonPropertyName("fault"), YamlMember(Order = 3, Alias = "fault")]
    public virtual WorkflowFault? Fault { get; set; }

    /// <inheritdoc/>
    [DataMember(Order = 999, Name = "extensionData"), JsonExtensionData]
    public virtual IDictionary<string, object>? ExtensionData { get; set; }

}