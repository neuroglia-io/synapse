namespace Synapse.Resources;

/// <summary>
/// Represents an object used to describe the status of a workflow activity
/// </summary>
[DataContract]
public record WorkflowActivityStatus
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowActivityStatus"/>
    /// </summary>
    public WorkflowActivityStatus() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowActivityStatus"/>
    /// </summary>
    /// <param name="phase">A value describing the current phase of the described activity. Values supported by default are: pending, running, sleeping, waiting, faulted, cancelled, executed</param>
    public WorkflowActivityStatus(string phase)
    {
        if (string.IsNullOrWhiteSpace(phase)) throw new ArgumentNullException(nameof(phase));
        if (phase.Length < 3 || phase.Length > 22 || !phase.IsAlphanumeric('-')) throw new ArgumentException($"The specified value '{phase}' is not a valid workflow activity phase", nameof(phase));
        this.Phase = phase;
    }

    /// <summary>
    /// Gets/sets a value describing the current phase of the described activity. Values supported by default are: pending, running, sleeping, waiting, faulted, cancelled, executed
    /// </summary>
    [Required, MinLength(3), MaxLength(22)]
    [DataMember(Order = 1, Name = "phase", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("phase"), YamlMember(Order = 1, Alias = "phase")]
    public virtual string Phase { get; set; } = null!;

    /// <summary>
    /// Gets/sets an object used to describe the error that has faulted the execution of the process, if any
    /// </summary>
    [DataMember(Order = 2, Name = "error"), JsonPropertyOrder(2), JsonPropertyName("error"), YamlMember(Order = 2, Alias = "error")]
    public virtual ProblemDetails? Error { get; set; }

    /// <summary>
    /// Gets/sets a reference to the process's output data, if any. If set, must be a valid url following the format '{storageProvider}://{storageSpecificReference}' (ex: minio://workflows/pets/order/po-2023Q200069.output.json)
    /// </summary>
    [DataMember(Order = 3, Name = "outputData", IsRequired = true), JsonPropertyOrder(3), JsonPropertyName("outputData"), YamlMember(Order = 3, Alias = "outputData")]
    public virtual Uri? OutputData { get; set; }

}