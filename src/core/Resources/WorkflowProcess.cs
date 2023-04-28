namespace Synapse.Resources;

/// <summary>
/// Represents a resource used to define a workflow process
/// </summary>
[DataContract]
public record WorkflowProcess
    : Resource<WorkflowProcessSpec, WorkflowProcessStatus>
{

    /// <summary>
    /// Gets/sets the resource group <see cref="WorkflowProcess"/>s belong to
    /// </summary>
    public static string ResourceGroup => ResourceDefinition.Spec.Group;
    /// <summary>
    /// Gets/sets the resource version of <see cref="WorkflowProcess"/>s
    /// </summary>
    public static string ResourceVersion => ResourceDefinition.Spec.Versions.Single(v => v.Storage).Name;
    /// <summary>
    /// Gets/sets the resource plural name of <see cref="WorkflowProcess"/>s
    /// </summary>
    public static string ResourcePlural => ResourceDefinition.Spec.Names.Plural;
    /// <summary>
    /// Gets/sets the resource kind of <see cref="WorkflowProcess"/>s
    /// </summary>
    public static string ResourceKind => ResourceDefinition.Spec.Names.Kind;

    /// <summary>
    /// Gets the <see cref="WorkflowProcess"/>'s resource type.
    /// </summary>
    public static ResourceDefinition ResourceDefinition { get; set; } = Hylo.Serializer.Yaml.Deserialize<ResourceDefinition>(EmbeddedResources.ReadToEnd(EmbeddedResources.Assets.Definitions.WorkflowProcess))!;

    /// <summary>
    /// Initializes a new <see cref="WorkflowProcess"/>
    /// </summary>
    public WorkflowProcess() : base(ResourceDefinition!) { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowProcess"/>
    /// </summary>
    /// <param name="metadata">The <see cref="WorkflowProcess"/>'s metadata</param>
    /// <param name="spec">The <see cref="WorkflowProcess"/>'s <see cref="WorkflowProcessSpec"/></param>
    /// <param name="status">The <see cref="WorkflowProcess"/>'s <see cref="WorkflowProcessStatus"/></param>
    public WorkflowProcess(ResourceMetadata metadata, WorkflowProcessSpec spec, WorkflowProcessStatus status)
        : base(ResourceDefinition!, metadata, spec, status)
    {

    }

}
