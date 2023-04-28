namespace Synapse.Resources;

/// <summary>
/// Represents an object used to define an extension to a workflow DSL schema
/// </summary>
[DataContract]
public record WorkflowDslSchemaExtension
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowDslSchemaExtension"/>
    /// </summary>
    public WorkflowDslSchemaExtension() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowDslSchemaExtension"/>
    /// </summary>
    /// <param name="patch">The patch to apply to the extended workflow DSL to customize its schema</param>
    public WorkflowDslSchemaExtension(Patch patch)
    {
        this.Patch = patch ?? throw new ArgumentNullException(nameof(patch));
    }

    /// <summary>
    /// Gets/sets the patch to apply to the extended workflow DSL to customize its schema
    /// </summary>
    [Required]
    [DataMember(Order = 1, Name = "patch", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("patch"), YamlMember(Order = 1, Alias = "patch")]
    public virtual Patch Patch { get; set; } = null!;

}