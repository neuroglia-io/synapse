namespace Synapse.Resources;

/// <summary>
/// Represents an object used to configure a workflow activity
/// </summary>
[DataContract]
public record WorkflowActivitySpec
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowActivitySpec"/>
    /// </summary>
    public WorkflowActivitySpec() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowActivitySpec"/>
    /// </summary>
    /// <param name="parameters">An object that contains the activity's DSL-specific parameters, if any</param>
    public WorkflowActivitySpec(IDictionary<string, object>? parameters = null)
    {
        this.Parameters = parameters;
    }

    /// <summary>
    /// Gets/sets an object that contains the activity's DSL-specific parameters, if any
    /// </summary>
    [DataMember(Order = 1, Name = "parameters"), JsonPropertyOrder(1), JsonPropertyName("parameters"), YamlMember(Order = 1, Alias = "parameters")]
    public virtual IDictionary<string, object>? Parameters { get; set; }

}
