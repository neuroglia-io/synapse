namespace Synapse.Resources;

/// <summary>
/// Represents an object used to configure a workflow language
/// </summary>
[DataContract]
public record WorkflowDslSpec
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowDslSpec"/>
    /// </summary>
    public WorkflowDslSpec() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowDslSpec"/>
    /// </summary>
    /// <param name="versions">An array containing the versions of the configured workflow DSL</param>
    public WorkflowDslSpec(params WorkflowDslVersion[] versions)
    {
        if (versions == null || versions.Length < 1) throw new ArgumentNullException(nameof(versions));
        this.Versions = versions.ToList();
    }

    /// <summary>
    /// Gets/sets a list containing all versions of the configured workflow DSL
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "versions", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("versions"), YamlMember(Order = 1, Alias = "versions")]
    public virtual List<WorkflowDslVersion> Versions { get; set; } = new();

}
