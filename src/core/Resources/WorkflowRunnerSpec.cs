namespace Synapse.Resources;

/// <summary>
/// Represents an object used to configure a workflow runner
/// </summary>
[DataContract]
public record WorkflowRunnerSpec
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowRunnerSpec"/>
    /// </summary>
    public WorkflowRunnerSpec() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowRunnerSpec"/>
    /// </summary>
    /// <param name="type">The runner's type. Supported values are: 'resource-controller', 'hosted-process'</param>
    /// <param name="supportedLanguages">A list containing references to the workflow Domain Specific Languages supported by the runner</param>
    public WorkflowRunnerSpec(string type, List<string> supportedLanguages)
    {
        if(string.IsNullOrWhiteSpace(type)) throw new ArgumentNullException(nameof(type));
        if (type.Length < 3 || type.Length > 22 || !type.IsAlphanumeric('-')) throw new ArgumentException($"The specified value '{type}' is not a valid workflow runner type", nameof(type));
        if(supportedLanguages == null || supportedLanguages.Count < 1) throw new ArgumentNullException(nameof(supportedLanguages));
        this.Type = type;
        this.SupportedLanguages = supportedLanguages;
    }

    /// <summary>
    /// Gets/sets the runner's type. Supported values are: 'resource-controller', 'hosted-process'
    /// </summary>
    [Required, MinLength(3), MaxLength(22)]
    [DataMember(Order = 1, Name = "type", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("type"), YamlMember(Order = 1, Alias = "type")]
    public virtual string Type { get; set; } = null!;

    /// <summary>
    /// Gets/sets a list containing references to the workflow Domain Specific Languages supported by the runner. 
    /// Values are concatenations of the name and the version of the supported language, and follows the format '{name}:{version}'. 
    /// The DSL name must contain a maximum of 63 lowercased alphanumeric characters or the '-' character. 
    /// The DSL version must contain a maximum of 22 alphanumeric characters or the '-' character. 
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Order = 2, Name = "supportedLanguages", IsRequired = true), JsonPropertyOrder(2), JsonPropertyName("supportedLanguages"), YamlMember(Order = 2, Alias = "supportedLanguages")]
    public virtual List<string> SupportedLanguages { get; set; } = null!;

}
