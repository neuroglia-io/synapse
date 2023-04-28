namespace Synapse.Resources;

/// <summary>
/// Represents an object used to define a specific version of a workflow DSL extension
/// </summary>
[DataContract]
public record WorkflowDslExtensionVersion
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowDslExtensionVersion"/>
    /// </summary>
    public WorkflowDslExtensionVersion() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowDslExtensionVersion"/>
    /// </summary>
    /// <param name="name">The name of the workflow DSL extension version</param>
    /// <param name="supportedVersions">A list of the DSL versions the defined extension version applies to. If null, applies to all versions of the extended DSL</param>
    /// <param name="schemaExtension">The schema extension to apply to the extended DSL, if any</param>
    public WorkflowDslExtensionVersion(string name, List<string>? supportedVersions = null, WorkflowDslSchemaExtension? schemaExtension = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (!SemVersion.TryParse(name, SemVersionStyles.Strict, out _)) throw new ArgumentException($"The specified value '{name}' is not a valid semantic version 2.0", nameof(name));
        this.Name = name;
        this.SupportedVersions = supportedVersions;
        this.SchemaExtension = schemaExtension;
    }

    /// <summary>
    /// Gets/sets the name of the workflow DSL extension version. Must contain a maximum of 22 alphanumeric characters or the '-' character. Must be a valid semantic version (https://semver.org/#semantic-versioning-200) such as '1.0.2-alpha1'
    /// </summary>
    [Required, MinLength(3), MaxLength(22), SemanticVersion]
    [DataMember(Order = 1, Name = "name", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("name"), YamlMember(Order = 1, Alias = "name")]
    public virtual string Name { get; set; } = null!;

    /// <summary>
    /// Gets/sets a list of the DSL versions the defined extension version applies to. If null, applies to all versions of the extended DSL.
    /// </summary>
    [DataMember(Order = 2, Name = "supportedVersions"), JsonPropertyOrder(2), JsonPropertyName("supportedVersions"), YamlMember(Order = 2, Alias = "supportedVersions")]
    public virtual List<string>? SupportedVersions { get; set; }

    /// <summary>
    /// Gets/sets the schema extension to apply to the extended DSL, if any
    /// </summary>
    [DataMember(Order = 3, Name = "schemaExtension"), JsonPropertyOrder(3), JsonPropertyName("schemaExtension"), YamlMember(Order = 3, Alias = "schemaExtension")]
    public virtual WorkflowDslSchemaExtension? SchemaExtension { get; set; }

}