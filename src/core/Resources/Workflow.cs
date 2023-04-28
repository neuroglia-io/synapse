namespace Synapse.Resources;

/// <summary>
/// Represents a resource used to define and configure a workflow
/// </summary>
[DataContract]
public record Workflow
    : Resource<WorkflowSpec>
{

    /// <summary>
    /// Gets/sets the resource group <see cref="Workflow"/>s belong to
    /// </summary>
    public static string ResourceGroup => ResourceDefinition.Spec.Group;
    /// <summary>
    /// Gets/sets the resource version of <see cref="Workflow"/>s
    /// </summary>
    public static string ResourceVersion => ResourceDefinition.Spec.Versions.Single(v => v.Storage).Name;
    /// <summary>
    /// Gets/sets the resource plural name of <see cref="Workflow"/>s
    /// </summary>
    public static string ResourcePlural => ResourceDefinition.Spec.Names.Plural;
    /// <summary>
    /// Gets/sets the resource kind of <see cref="Workflow"/>s
    /// </summary>
    public static string ResourceKind => ResourceDefinition.Spec.Names.Kind;

    /// <summary>
    /// Gets the <see cref="Workflow"/>'s resource type.
    /// </summary>
    public static ResourceDefinition ResourceDefinition { get; set; } = Hylo.Serializer.Yaml.Deserialize<ResourceDefinition>(EmbeddedResources.ReadToEnd(EmbeddedResources.Assets.Definitions.Workflow))!;

    /// <summary>
    /// Initializes a new <see cref="Workflow"/>
    /// </summary>
    public Workflow() : base(ResourceDefinition!) { }

    /// <summary>
    /// Initializes a new <see cref="Workflow"/>
    /// </summary>
    /// <param name="metadata">The <see cref="Workflow"/>'s metadata</param>
    /// <param name="spec">The <see cref="Workflow"/>'s <see cref="WorkflowSpec"/></param>
    public Workflow(ResourceMetadata metadata, WorkflowSpec spec)
        : base(ResourceDefinition!, metadata, spec)
    {

    }

}
