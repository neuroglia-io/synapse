namespace Synapse.Resources;

/// <summary>
/// Represents an object ysed to define a specific version of a workflow DSL
/// </summary>
[DataContract]
public record WorkflowDslVersion
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowDslVersion"/>
    /// </summary>
    public WorkflowDslVersion() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowDslVersion"/>
    /// </summary>
    /// <param name="name">The name of the language version</param>
    /// <param name="schema">The schema that all definitions written in the configured language version must adhere to</param>
    public WorkflowDslVersion(string name, JsonSchema schema)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (!SemVersion.TryParse(name, SemVersionStyles.Strict, out _)) throw new ArgumentException($"The specified value '{name}' is not a valid semantic version 2.0", nameof(name));
        this.Name = name;
        this.Schema = schema ?? throw new ArgumentNullException(nameof(schema));
    }

    /// <summary>
    /// Gets/sets the name of the language version. Must contain a maximum of 22 alphanumeric characters or the '-' character. Must be a valid semantic version (https://semver.org/#semantic-versioning-200) such as '1.0.2-alpha1'
    /// </summary>
    [Required, MinLength(3), MaxLength(22), SemanticVersion]
    [DataMember(Order = 1, Name = "name", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("name"), YamlMember(Order = 1, Alias = "name")]
    public virtual string Name { get; set; } = null!;

    /// <summary>
    /// Gets/sets the schema that all definitions written in the configured language version must adhere to
    /// </summary>
    [DataMember(Order = 2, Name = "schema", IsRequired = true), JsonPropertyOrder(2), JsonPropertyName("schema"), YamlMember(Order = 2, Alias = "schema")]
    public virtual JsonSchema Schema { get; set; } = null!;

}