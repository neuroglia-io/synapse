namespace Synapse.Resources;

/// <summary>
/// Represents an object used to define an extension to a workflow DSL
/// </summary>
[DataContract]
public record WorkflowDslExtension
    : Resource<WorkflowDslExtensionSpec>
{

    /// <summary>
    /// Gets/sets the resource group <see cref="WorkflowDslExtension"/>s belong to
    /// </summary>
    public static string ResourceGroup => ResourceDefinition.Spec.Group;
    /// <summary>
    /// Gets/sets the resource version of <see cref="WorkflowDslExtension"/>s
    /// </summary>
    public static string ResourceVersion => ResourceDefinition.Spec.Versions.Single(v => v.Storage).Name;
    /// <summary>
    /// Gets/sets the resource plural name of <see cref="WorkflowDslExtension"/>s
    /// </summary>
    public static string ResourcePlural => ResourceDefinition.Spec.Names.Plural;
    /// <summary>
    /// Gets/sets the resource kind of <see cref="WorkflowDslExtension"/>s
    /// </summary>
    public static string ResourceKind => ResourceDefinition.Spec.Names.Kind;

    /// <summary>
    /// Gets the <see cref="WorkflowDslExtension"/>'s resource type.
    /// </summary>
    public static ResourceDefinition ResourceDefinition { get; set; } = Hylo.Serializer.Yaml.Deserialize<ResourceDefinition>(EmbeddedResources.ReadToEnd(EmbeddedResources.Assets.Definitions.WorkflowDslExtension))!;

    /// <summary>
    /// Initializes a new <see cref="WorkflowDslExtension"/>
    /// </summary>
    public WorkflowDslExtension() : base(ResourceDefinition!) { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowDslExtension"/>
    /// </summary>
    /// <param name="metadata">The <see cref="WorkflowDslExtension"/>'s metadata</param>
    /// <param name="spec">The <see cref="WorkflowDslExtension"/>'s <see cref="WorkflowDslExtensionSpec"/></param>
    public WorkflowDslExtension(ResourceMetadata metadata, WorkflowDslExtensionSpec spec)
        : base(ResourceDefinition!, metadata, spec)
    {

    }


}