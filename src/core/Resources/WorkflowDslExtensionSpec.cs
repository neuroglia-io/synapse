namespace Synapse.Resources;

/// <summary>
/// Represents an object used to configure a workflow DSL extension
/// </summary>
[DataContract]
public record WorkflowDslExtensionSpec
{

    /// <summary>
    /// Gets/sets the name of the DSL the extension applies to. Must contain a maximum of 63 lowercased alphanumeric characters or the '-' character. 
    /// </summary>
    [Required, MinLength(3), MaxLength(63)]
    [DataMember(Order = 1, Name = "dsl", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("dsl"), YamlMember(Order = 1, Alias = "dsl")]
    public virtual string Dsl { get; set; } = null!;

    /// <summary>
    /// Gets/sets a list containing all versions of the configured workflow DSL extension
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Order = 2, Name = "versions", IsRequired = true), JsonPropertyOrder(2), JsonPropertyName("versions"), YamlMember(Order = 2, Alias = "versions")]
    public virtual List<WorkflowDslExtensionVersion> Versions { get; set; } = new();

}
