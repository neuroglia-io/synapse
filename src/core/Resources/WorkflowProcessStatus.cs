namespace Synapse.Resources;

/// <summary>
/// Represents an object used to describe the status of a workflow process
/// </summary>
[DataContract]
public record WorkflowProcessStatus
{

    /// <summary>
    /// Gets/sets the workflow process's phase. Values supported by default are 'pending', 'running', 'suspended', 'faulted', 'cancelled', 'executed'
    /// </summary>
    [Required, MinLength(3), MaxLength(22)]
    [DataMember(Order = 1, Name = "phase", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("phase"), YamlMember(Order = 1, Alias = "phase")]
    public virtual string Phase { get; set; } = null!;

    /// <summary>
    /// Gets/sets a reference to the runner that is currently executing the described workflow process, if any
    /// </summary>
    [DataMember(Order = 2, Name = "runner"), JsonPropertyOrder(2), JsonPropertyName("runner"), YamlMember(Order = 2, Alias = "runner")]
    public virtual string? Runner { get; set; }

    /// <summary>
    /// Gets/sets a list containing the activities processed during the described workflow process's execution
    /// </summary>
    [DataMember(Order = 3, Name = "activities", IsRequired = true), JsonPropertyOrder(5), JsonPropertyName("activities"), YamlMember(Order = 5, Alias = "activities")]
    public virtual List<WorkflowActivity>? Activities { get; set; }

    /// <summary>
    /// Gets/sets an object used to describe the error that has faulted the execution of the process, if any
    /// </summary>
    [DataMember(Order = 4, Name = "error"), JsonPropertyOrder(3), JsonPropertyName("error"), YamlMember(Order = 3, Alias = "error")]
    public virtual ProblemDetails? Error { get; set; }

    /// <summary>
    /// Gets/sets a reference to the process's output data, if any. If set, must be a valid url following the format '{storageProvider}://{storageSpecificReference}' (ex: minio://workflows/pets/order/po-2023Q200069.output.json)
    /// </summary>
    [DataMember(Order = 5, Name = "outputData", IsRequired = true), JsonPropertyOrder(4), JsonPropertyName("outputData"), YamlMember(Order = 4, Alias = "outputData")]
    public virtual Uri? OutputData { get; set; }


}