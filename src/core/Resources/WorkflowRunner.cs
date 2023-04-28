namespace Synapse.Resources;

/// <summary>
/// Represents a resource used to define a workflow runner
/// </summary>
[DataContract]
public record WorkflowRunner
    : Resource<WorkflowRunnerSpec>
{

    /// <summary>
    /// Gets/sets the resource group <see cref="WorkflowRunner"/>s belong to
    /// </summary>
    public static string ResourceGroup => ResourceDefinition.Spec.Group;
    /// <summary>
    /// Gets/sets the resource version of <see cref="WorkflowRunner"/>s
    /// </summary>
    public static string ResourceVersion => ResourceDefinition.Spec.Versions.Single(v => v.Storage).Name;
    /// <summary>
    /// Gets/sets the resource plural name of <see cref="WorkflowRunner"/>s
    /// </summary>
    public static string ResourcePlural => ResourceDefinition.Spec.Names.Plural;
    /// <summary>
    /// Gets/sets the resource kind of <see cref="WorkflowRunner"/>s
    /// </summary>
    public static string ResourceKind => ResourceDefinition.Spec.Names.Kind;

    /// <summary>
    /// Gets the <see cref="WorkflowRunner"/>'s resource type.
    /// </summary>
    public static ResourceDefinition ResourceDefinition { get; set; } = Hylo.Serializer.Yaml.Deserialize<ResourceDefinition>(EmbeddedResources.ReadToEnd(EmbeddedResources.Assets.Definitions.WorkflowRunner))!;

    /// <summary>
    /// Initializes a new <see cref="WorkflowRunner"/>
    /// </summary>
    public WorkflowRunner() : base(ResourceDefinition!) { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowRunner"/>
    /// </summary>
    /// <param name="metadata">The <see cref="WorkflowRunner"/>'s metadata</param>
    /// <param name="spec">The <see cref="WorkflowRunner"/>'s <see cref="WorkflowRunnerSpec"/></param>
    public WorkflowRunner(ResourceMetadata metadata, WorkflowRunnerSpec spec)
        : base(ResourceDefinition!, metadata, spec)
    {

    }

}
