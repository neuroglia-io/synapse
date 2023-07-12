namespace Synapse.Resources;

/// <summary>
/// Represents a resource used to define a workflow agent, which is an autonomous application that processes workflow instances
/// </summary>
[DataContract]
public record WorkflowAgent
    : Resource<WorkflowAgentSpec>
{

    /// <summary>
    /// Gets/sets the resource group <see cref="WorkflowAgent"/>s belong to
    /// </summary>
    public static string ResourceGroup => ResourceDefinition.Spec.Group;
    /// <summary>
    /// Gets/sets the resource version of <see cref="WorkflowAgent"/>s
    /// </summary>
    public static string ResourceVersion => ResourceDefinition.Spec.Versions.Single(v => v.Storage).Name;
    /// <summary>
    /// Gets/sets the resource plural name of <see cref="WorkflowAgent"/>s
    /// </summary>
    public static string ResourcePlural => ResourceDefinition.Spec.Names.Plural;
    /// <summary>
    /// Gets/sets the resource kind of <see cref="WorkflowAgent"/>s
    /// </summary>
    public static string ResourceKind => ResourceDefinition.Spec.Names.Kind;

    /// <summary>
    /// Gets the <see cref="WorkflowAgent"/>'s resource type.
    /// </summary>
    public static ResourceDefinition ResourceDefinition { get; set; } = Hylo.Serializer.Yaml.Deserialize<ResourceDefinition>(EmbeddedResources.ReadToEnd(EmbeddedResources.Assets.Definitions.WorkflowAgent))!;

    /// <summary>
    /// Initializes a new <see cref="WorkflowAgent"/>
    /// </summary>
    public WorkflowAgent() : base(ResourceDefinition!) { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowAgent"/>
    /// </summary>
    /// <param name="metadata">The <see cref="WorkflowAgent"/>'s metadata</param>
    /// <param name="spec">The <see cref="WorkflowAgent"/>'s <see cref="WorkflowAgentSpec"/></param>
    public WorkflowAgent(ResourceMetadata metadata, WorkflowAgentSpec spec)
        : base(ResourceDefinition!, metadata, spec)
    {

    }

}
