namespace Synapse.Resources;

/// <summary>
/// Represents an object used to define a specific workflow version
/// </summary>
[DataContract]
public record WorkflowVersion
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowVersion"/>
    /// </summary>
    public WorkflowVersion() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowVersion"/>
    /// </summary>
    /// <param name="name">The name of the workflow version</param>
    /// <param name="definition">The definition of the workflow, expressed in the specified workflow language</param>
    public WorkflowVersion(string name, JsonObject definition)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (!SemVersion.TryParse(name, SemVersionStyles.Strict, out _)) throw new ArgumentException($"The specified value '{name}' is not a valid semantic version 2.0", nameof(name));
        this.Name = name;
        this.Definition = definition ?? throw new ArgumentNullException(nameof(definition));
    }

    /// <summary>
    /// Gets/sets the name of the workflow version. Must contain a maximum of 22 alphanumeric characters or the '-' character. Must be a valid semantic version (https://semver.org/#semantic-versioning-200) such as '1.0.2-alpha1'
    /// </summary>
    [Required, MinLength(3), MaxLength(22), SemanticVersion]
    [DataMember(Order = 1, Name = "name", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("name"), YamlMember(Order = 1, Alias = "name")]
    public virtual string Name { get; set; } = null!;

    /// <summary>
    /// Gets/sets the definition of the workflow, expressed in the specified workflow language
    /// </summary>
    [DataMember(Order = 2, Name = "definition", IsRequired = true), JsonPropertyOrder(2), JsonPropertyName("definition"), YamlMember(Order = 2, Alias = "definition")]
    public virtual JsonObject Definition { get; set; } = null!;

    /// <summary>
    /// Gets/sets a boolean indicating whether or not the defined version has been retired and is no longer instantiable
    /// </summary>
    [DataMember(Order = 3, Name = "retired", IsRequired = true), JsonPropertyOrder(3), JsonPropertyName("retired"), YamlMember(Order = 3, Alias = "retired")]
    public virtual bool Retired { get; set; }

}
