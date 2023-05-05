namespace Synapse.Resources;

/// <summary>
/// Represents an object used to define the processes created by an agent to run workflow instances matching the configured criteria
/// </summary>
[DataContract]
public record WorkflowAgentProcessRule
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowAgentProcessRule"/>
    /// </summary>
    public WorkflowAgentProcessRule() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowAgentProcessRule"/>
    /// </summary>
    /// <param name="name">The rule's name. Must contains between 3 and 22 alphanumeric characters and the '-' character. Must be unique amongst other rules</param>
    /// <param name="supportedLanguages"></param>
    /// <param name="process">A list containing references to the workflow Domain Specific Languages supported by the configured process rule</param>
    /// <param name="selectors">A list containing the label selectors used to select workflow instances by, if any</param>
    public WorkflowAgentProcessRule(string name, IEnumerable<string> supportedLanguages, ProcessConfiguration process, IEnumerable<LabelSelector>? selectors = null)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (supportedLanguages == null || !supportedLanguages.Any()) throw new ArgumentNullException(nameof(supportedLanguages));
        supportedLanguages.All(l => (l.Length < 7 || l.Length > 126 || !l.IsAlphanumeric('-', '.') || l.Split('.').Length != 2) ? throw new ArgumentException($"The specified value '{l}' is not a valid versioned DSL reference", nameof(supportedLanguages)) : true);
        this.Name = name;
        this.SupportedLanguages = new(supportedLanguages);
        this.Selectors = selectors == null ? null : new(selectors);
        this.Process = process ?? throw new ArgumentNullException(nameof(process));
    }

    /// <summary>
    /// Gets/sets the rule's name. Must contains between 3 and 22 alphanumeric characters and the '-' character. Must be unique amongst other rules
    /// </summary>
    [Required, MinLength(3), MaxLength(22)]
    [DataMember(Order = 1, Name = "name", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("name"), YamlMember(Order = 1, Alias = "name")]
    public virtual string Name { get; set; } = null!;

    /// <summary>
    /// Gets/sets a list containing references to the workflow Domain Specific Languages supported by the configured process rule. 
    /// Values are concatenations of the name and the version of the supported DSL, and follows the format '{name}:{version}'. 
    /// The DSL name must contain a maximum of 63 lowercased alphanumeric characters or the '-' character. 
    /// The DSL version must contain a maximum of 22 alphanumeric characters or the '-' character. 
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Order = 2, Name = "supportedLanguages", IsRequired = true), JsonPropertyOrder(2), JsonPropertyName("supportedLanguages"), YamlMember(Order = 2, Alias = "supportedLanguages")]
    public virtual EquatableList<string> SupportedLanguages { get; set; } = null!;

    /// <summary>
    /// Gets/sets a list containing the label selectors used to select workflow instances by, if any
    /// </summary>
    [DataMember(Order = 3, Name = "selectors", IsRequired = true), JsonPropertyOrder(3), JsonPropertyName("selectors"), YamlMember(Order = 3, Alias = "selectors")]
    public virtual EquatableList<LabelSelector>? Selectors { get; set; }

    /// <summary>
    /// Gets/sets an object used to configure the processes for running workflow instances that match the rule
    /// </summary>
    [DataMember(Order = 4, Name = "process", IsRequired = true), JsonPropertyOrder(4), JsonPropertyName("process"), YamlMember(Order = 4, Alias = "process")]
    public virtual ProcessConfiguration Process { get; set; } = null!;

}