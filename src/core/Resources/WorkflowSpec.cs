namespace Synapse.Resources;

/// <summary>
/// Represents an object used to configure a workflow
/// </summary>
[DataContract]
public record WorkflowSpec
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowSpec"/>
    /// </summary>
    public WorkflowSpec() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowSpec"/>
    /// </summary>
    /// <param name="dsl">A a reference to the language the workflow is written in</param>
    /// <param name="versions">An array containing the versions of the configured workflow</param>
    public WorkflowSpec(string dsl, params WorkflowVersion[] versions)
    {
        if (string.IsNullOrWhiteSpace(dsl)) throw new ArgumentNullException(nameof(dsl));
        if (WorkflowDsl.IsValidReference(dsl)) throw new ArgumentException($"The specified value '{dsl}' is not a valid workflow language reference", nameof(dsl));
        if(versions == null || versions.Length < 1) throw new ArgumentNullException(nameof(versions));
        this.Dsl = dsl;
        this.Versions = new(versions);
    }

    /// <summary>
    /// Gets/sets a reference to the language the workflow is written in. The value is the concatenation of the name and the version of the language, and follows the format '{name}:{version}'. 
    /// The name must contain a maximum of 63 lowercased alphanumeric characters or the '-' character. 
    /// The version must contain a maximum of 22 alphanumeric characters or the '-' character. 
    /// </summary>
    [Required, MinLength(7), MaxLength(85)]
    [DataMember(Order = 1, Name = "dsl", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("dsl"), YamlMember(Order = 1, Alias = "dsl")]
    public virtual string Dsl { get; set; } = null!;

    /// <summary>
    /// Gets/sets a list containing all versions of the configured workflow
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Order = 2, Name = "versions", IsRequired = true), JsonPropertyOrder(2), JsonPropertyName("versions"), YamlMember(Order = 2, Alias = "versions")]
    public virtual EquatableList<WorkflowVersion> Versions { get; set; } = new();

}
