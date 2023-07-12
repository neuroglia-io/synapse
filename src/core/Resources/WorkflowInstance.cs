namespace Synapse.Resources;

/// <summary>
/// Represents a resource used to define an instance of a workflow
/// </summary>
[DataContract]
public record WorkflowInstance
    : Resource<WorkflowInstanceSpec, WorkflowInstanceStatus>
{

    /// <summary>
    /// Gets/sets the resource group <see cref="WorkflowInstance"/>s belong to
    /// </summary>
    public static string ResourceGroup => ResourceDefinition.Spec.Group;
    /// <summary>
    /// Gets/sets the resource version of <see cref="WorkflowInstance"/>s
    /// </summary>
    public static string ResourceVersion => ResourceDefinition.Spec.Versions.Single(v => v.Storage).Name;
    /// <summary>
    /// Gets/sets the resource plural name of <see cref="WorkflowInstance"/>s
    /// </summary>
    public static string ResourcePlural => ResourceDefinition.Spec.Names.Plural;
    /// <summary>
    /// Gets/sets the resource kind of <see cref="WorkflowInstance"/>s
    /// </summary>
    public static string ResourceKind => ResourceDefinition.Spec.Names.Kind;

    /// <summary>
    /// Gets the <see cref="WorkflowInstance"/>'s resource type.
    /// </summary>
    public static ResourceDefinition ResourceDefinition { get; set; } = Hylo.Serializer.Yaml.Deserialize<ResourceDefinition>(EmbeddedResources.ReadToEnd(EmbeddedResources.Assets.Definitions.WorkflowInstance))!;

    /// <summary>
    /// Initializes a new <see cref="WorkflowInstance"/>
    /// </summary>
    public WorkflowInstance() : base(ResourceDefinition!) { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowInstance"/>
    /// </summary>
    /// <param name="metadata">The <see cref="WorkflowInstance"/>'s metadata</param>
    /// <param name="spec">The <see cref="WorkflowInstance"/>'s <see cref="WorkflowInstanceSpec"/></param>
    /// <param name="status">The <see cref="WorkflowInstance"/>'s <see cref="WorkflowInstanceStatus"/></param>
    public WorkflowInstance(ResourceMetadata metadata, WorkflowInstanceSpec spec, WorkflowInstanceStatus status)
        : base(ResourceDefinition!, metadata, spec, status)
    {

    }

    /// <summary>
    /// Exposes <see cref="WorkflowInstance"/>-related labels
    /// </summary>
    public static class Labels
    {

        /// <summary>
        /// Gets the label used to reference the runner that is managing a workflow instance
        /// </summary>
        public const string Agent = SynapseConstants.Resources.LabelPrefix + "agent";

    }

}
