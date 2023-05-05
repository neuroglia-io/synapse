namespace Synapse.Resources;

/// <summary>
/// Represents an object used to configure a workflow agent
/// </summary>
[DataContract]
public record WorkflowAgentSpec
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowAgentSpec"/>
    /// </summary>
    public WorkflowAgentSpec() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowAgentSpec"/>
    /// </summary>
    /// <param name="processRules">A set of rules that define which workflow instances to run and how</param>
    public WorkflowAgentSpec(List<WorkflowAgentProcessRule> processRules)
    {
        if(processRules == null || processRules.Count < 1) throw new ArgumentNullException(nameof(processRules));
        this.ProcessRules = new(processRules);
    }

    /// <summary>
    /// Gets/sets a set of rules that define which workflow instances to run and how
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "processRules", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("processRules"), YamlMember(Order = 1, Alias = "processRules")]
    public virtual EquatableList<WorkflowAgentProcessRule> ProcessRules { get; set; } = null!;

}
