namespace Synapse.Resources;

/// <summary>
/// Represents an object used to describe a period of time during which a workflow instance has been running
/// </summary>
public record WorkflowInstanceRuntime
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowInstanceRuntime"/>
    /// </summary>
    public WorkflowInstanceRuntime() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowInstanceRuntime"/>
    /// </summary>
    /// <param name="agentQualifiedName">The qualified name of the agent managing the execution of the workflow instance</param>
    /// <param name="processId">A value used to globally and uniquely identify the process that is running the workflow instance</param>
    public WorkflowInstanceRuntime(string agentQualifiedName, string processId)
    {
        if (string.IsNullOrWhiteSpace(agentQualifiedName)) throw new ArgumentNullException(nameof(agentQualifiedName));
        if (string.IsNullOrWhiteSpace(processId)) throw new ArgumentNullException(nameof(processId));
        this.Agent = agentQualifiedName;
        this.ProcessId = processId;
        this.StartedAt = DateTimeOffset.Now;
    }

    /// <summary>
    /// Gets/sets the qualified name of the agent managing the execution of the workflow instance
    /// </summary>
    [Required]
    [DataMember(Order = 1, Name = "agent", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("agent"), YamlMember(Order = 1, Alias = "agent")]
    public virtual string Agent { get; set; } = null!;

    /// <summary>
    /// Gets/sets a value used to globally and uniquely identify the process that is running the workflow instance
    /// </summary>
    [Required]
    [DataMember(Order = 2, Name = "processId", IsRequired = true), JsonPropertyOrder(2), JsonPropertyName("processId"), YamlMember(Order = 2, Alias = "processId")]
    public virtual string ProcessId { get; set; } = null!;

    /// <summary>
    /// Gets the date and time at which the session has started
    /// </summary>
    [Required]
    [DataMember(Order = 3, Name = "startedAt", IsRequired = true), JsonPropertyOrder(3), JsonPropertyName("startedAt"), YamlMember(Order = 3, Alias = "startedAt")]
    public virtual DateTimeOffset StartedAt { get; set; }

    /// <summary>
    /// Gets the date and time at which the session has ended
    /// </summary>
    [DataMember(Order = 4, Name = "endedAt"), JsonPropertyOrder(4), JsonPropertyName("endedAt"), YamlMember(Order = 4, Alias = "endedAt")]
    public virtual DateTimeOffset? EndedAt { get; set; }

}
