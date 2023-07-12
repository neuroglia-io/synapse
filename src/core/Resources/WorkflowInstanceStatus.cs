namespace Synapse.Resources;

/// <summary>
/// Represents an object used to describe the status of a workflow instance
/// </summary>
[DataContract]
public record WorkflowInstanceStatus
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowInstanceStatus"/>
    /// </summary>
    public WorkflowInstanceStatus() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowInstanceStatus"/>
    /// </summary>
    /// <param name="agent">A reference to the agent that is currently executing the described workflow instance, if any</param>
    public WorkflowInstanceStatus(string agent)
    {
        if (string.IsNullOrWhiteSpace(agent)) throw new ArgumentNullException(nameof(agent));
        this.Phase = WorkflowInstancePhase.Pending;
        this.Agent = agent;
    }

    /// <summary>
    /// Gets/sets the workflow instance's phase. Values supported by default are 'pending', 'running', 'suspended', 'faulted', 'cancelled', 'executed'
    /// </summary>
    [Required, MinLength(3), MaxLength(22)]
    [DataMember(Order = 1, Name = "phase", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("phase"), YamlMember(Order = 1, Alias = "phase")]
    public virtual WorkflowInstancePhase Phase { get; set; }

    /// <summary>
    /// Gets/sets a reference to the agent that is currently executing the described workflow instance, if any
    /// </summary>
    [DataMember(Order = 2, Name = "agent"), JsonPropertyOrder(2), JsonPropertyName("agent"), YamlMember(Order = 2, Alias = "agent")]
    public virtual string? Agent { get; set; }

    /// <summary>
    /// Gets/sets a list containing the workflow instance's runtimes, if any
    /// </summary>
    [DataMember(Order = 3, Name = "runtimes"), JsonPropertyOrder(3), JsonPropertyName("runtimes"), YamlMember(Order = 3, Alias = "runtimes")]
    public virtual EquatableList<WorkflowInstanceRuntime>? Runtimes { get; set; }

    /// <summary>
    /// Gets/sets a list containing the activities processed during the described workflow instance's execution
    /// </summary>
    [DataMember(Order = 4, Name = "activities", IsRequired = true), JsonPropertyOrder(4), JsonPropertyName("activities"), YamlMember(Order = 4, Alias = "activities")]
    public virtual EquatableList<WorkflowActivity>? Activities { get; set; }

    /// <summary>
    /// Gets/sets an object used to describe the error that has faulted the execution of the instance, if any
    /// </summary>
    [DataMember(Order = 5, Name = "error"), JsonPropertyOrder(5), JsonPropertyName("error"), YamlMember(Order = 5, Alias = "error")]
    public virtual ProblemDetails? Error { get; set; }

    /// <summary>
    /// Gets/sets a reference to the instance's output data, if any. If set, must be a valid url following the format '{storageProvider}://{storageSpecificReference}' (ex: minio://workflows/pets/order/po-2023Q200069.output.json)
    /// </summary>
    [DataMember(Order = 6, Name = "outputData", IsRequired = true), JsonPropertyOrder(6), JsonPropertyName("outputData"), YamlMember(Order = 6, Alias = "outputData")]
    public virtual Uri? OutputData { get; set; }


}