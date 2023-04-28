namespace Synapse.Resources;

/// <summary>
/// Represents an object used to define a workflow activity
/// </summary>
[DataContract]
public record WorkflowActivity
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowActivity"/>
    /// </summary>
    public WorkflowActivity() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowActivity"/>
    /// </summary>
    /// <param name="metadata">An object used to hold information about the workflow activity</param>
    /// <param name="spec">An object used to configure the activity</param>
    /// <param name="status">An object used to describe the status of a workflow activity</param>
    public WorkflowActivity(WorkflowActivityMetadata metadata, WorkflowActivitySpec spec, WorkflowActivityStatus status)
    {
        this.Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        this.Spec = spec ?? throw new ArgumentNullException(nameof(spec));
        this.Status = status ?? throw new ArgumentNullException(nameof(status));
    }

    /// <summary>
    /// Gets/sets an object used to hold information about the workflow activity
    /// </summary>
    [Required]
    [DataMember(Order = 1, Name = "metadata", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("metadata"), YamlMember(Order = 1, Alias = "metadata")]
    public virtual WorkflowActivityMetadata Metadata { get; set; } = null!;

    /// <summary>
    /// Gets/sets an object used to configure the activity
    /// </summary>
    [Required]
    [DataMember(Order = 2, Name = "spec", IsRequired = true), JsonPropertyOrder(2), JsonPropertyName("spec"), YamlMember(Order = 2, Alias = "spec")]
    public virtual WorkflowActivitySpec Spec { get; set; } = null!;

    /// <summary>
    /// Gets/sets an object used to describe the status of the workflow activity
    /// </summary>
    [Required]
    [DataMember(Order = 3, Name = "status", IsRequired = true), JsonPropertyOrder(3), JsonPropertyName("status"), YamlMember(Order = 3, Alias = "status")]
    public virtual WorkflowActivityStatus Status { get; set; } = null!;

}
