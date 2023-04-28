namespace Synapse.Resources;

/// <summary>
/// Represents an object used to hold information about a workflow activity
/// </summary>
[DataContract]
public record WorkflowActivityMetadata
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowActivityMetadata"/>
    /// </summary>
    public WorkflowActivityMetadata() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowActivityMetadata"/>
    /// </summary>
    /// <param name="name">The name of the described activity</param>
    /// <param name="type">The DSL-specific type of the described activity</param>
    /// <param name="definition">A JSON pointer to the definition of the activity in the workflow version's definition document</param>
    public WorkflowActivityMetadata(string name, string type, JsonPointer definition)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (name.Length < 3 || name.Length > 63 || !name.IsAlphanumeric('-')) throw new ArgumentException($"The specified value '{name}' is not a valid workflow activity name", nameof(name));
        if(string.IsNullOrWhiteSpace(type)) throw new ArgumentNullException(nameof(type));
        this.Name = name;
        this.Type = type;
        this.Definition = definition ?? throw new ArgumentNullException(nameof(definition));
    }

    /// <summary>
    /// Gets/sets the activity's name
    /// </summary>
    [Required, MinLength(3), MaxLength(63)]
    [DataMember(Order = 1, Name = "name", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("name"), YamlMember(Order = 1, Alias = "name")]
    public virtual string Name { get; set; } = null!;

    /// <summary>
    /// Gets/sets the DSL-specific type of the activity
    /// </summary>
    [Required, MinLength(3), MaxLength(22)]
    [DataMember(Order = 2, Name = "type", IsRequired = true), JsonPropertyOrder(2), JsonPropertyName("type"), YamlMember(Order = 2, Alias = "type")]
    public virtual string Type { get; set; } = null!;

    /// <summary>
    /// Gets/sets a JSON pointer to the definition of the activity in the workflow version's definition document
    /// </summary>
    [Required]
    [DataMember(Order = 3, Name = "definition", IsRequired = true), JsonPropertyOrder(3), JsonPropertyName("definition"), YamlMember(Order = 3, Alias = "definition")]
    public virtual JsonPointer Definition { get; set; } = null!;

}
