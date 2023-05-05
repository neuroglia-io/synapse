namespace Synapse.Resources;

/// <summary>
/// Represents a reference to a workflow
/// </summary>
[DataContract]
public record WorkflowVersionReference
    : ResourceReference<Workflow>
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowVersionReference"/>
    /// </summary>
    public WorkflowVersionReference() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowVersionReference"/>
    /// </summary>
    /// <param name="name">The name of the referenced workflow</param>
    /// <param name="namespace">The namespace the referenced workflow belongs to</param>
    /// <param name="version">The name of the referenced workflow version</param>
    /// <exception cref="ArgumentNullException"></exception>
    public WorkflowVersionReference(string name, string @namespace, string version)
        : base(name, @namespace)
    {
        if (string.IsNullOrWhiteSpace(version)) throw new ArgumentNullException(nameof(version));
        ObjectNamingConvention.Current.EnsureIsValidVersion(version);
        this.Version = version;
    }

    /// <summary>
    /// Gets/sets the name of the referenced workflow version
    /// </summary>
    [Required, MinLength(3), MaxLength(22), SemanticVersion]
    [DataMember(Order = 10, Name = "version", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("version"), YamlMember(Order = 1, Alias = "version")]
    public virtual string Version { get; set; } = null!;

    /// <inheritdoc/>
    public override string ToString() => $"{this.Name}.{this.Namespace!}:{this.Version}";

    /// <summary>
    /// Parses the specified input into a new <see cref="WorkflowVersionReference"/> 
    /// </summary>
    /// <param name="input">The input to parse</param>
    public static implicit operator WorkflowVersionReference(string input) => Parse(input);

    /// <summary>
    /// Parses the specified input into a new <see cref="WorkflowVersionReference"/> 
    /// </summary>
    /// <param name="input">The input to parse</param>
    /// <returns>The parsed <see cref="WorkflowVersionReference"/></returns>
    public static WorkflowVersionReference Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) throw new ArgumentNullException(nameof(input));
        var components = input.Split(':', StringSplitOptions.RemoveEmptyEntries);
        if (components.Length != 2) throw new FormatException($"The specified value '{input}' is not a valid workflow version reference");
        var qualifiedName = components[0];
        var nameComponents = qualifiedName.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (nameComponents.Length != 2) throw new FormatException($"The specified value '{input}' is not a valid workflow version reference");
        var name = nameComponents[0];
        var @namespace = nameComponents[1];
        var version = components[1];
        return new(name, @namespace, version);
    }

}