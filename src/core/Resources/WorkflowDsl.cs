namespace Synapse.Resources;

/// <summary>
/// Represents a resource used to define a workflow DSL
/// </summary>
[DataContract]
public record WorkflowDsl
    : Resource<WorkflowDslSpec>
{

    /// <summary>
    /// Gets/sets the resource group <see cref="WorkflowDsl"/>s belong to
    /// </summary>
    public static string ResourceGroup => ResourceDefinition.Spec.Group;
    /// <summary>
    /// Gets/sets the resource version of <see cref="WorkflowDsl"/>s
    /// </summary>
    public static string ResourceVersion => ResourceDefinition.Spec.Versions.Single(v => v.Storage).Name;
    /// <summary>
    /// Gets/sets the resource plural name of <see cref="WorkflowDsl"/>s
    /// </summary>
    public static string ResourcePlural => ResourceDefinition.Spec.Names.Plural;
    /// <summary>
    /// Gets/sets the resource kind of <see cref="WorkflowDsl"/>s
    /// </summary>
    public static string ResourceKind => ResourceDefinition.Spec.Names.Kind;

    /// <summary>
    /// Gets the <see cref="Workflow"/>'s resource type.
    /// </summary>
    public static ResourceDefinition ResourceDefinition { get; set; } = Hylo.Serializer.Yaml.Deserialize<ResourceDefinition>(EmbeddedResources.ReadToEnd(EmbeddedResources.Assets.Definitions.WorkflowDsl))!;

    /// <summary>
    /// Initializes a new <see cref="WorkflowDsl"/>
    /// </summary>
    public WorkflowDsl() : base(ResourceDefinition!) { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowDsl"/>
    /// </summary>
    /// <param name="metadata">The <see cref="WorkflowDsl"/>'s metadata</param>
    /// <param name="spec">The <see cref="WorkflowDsl"/>'s <see cref="WorkflowDslSpec"/></param>
    public WorkflowDsl(ResourceMetadata metadata, WorkflowDslSpec spec)
        : base(ResourceDefinition!, metadata, spec)
    {

    }

    /// <summary>
    /// Determines whether or not the specified value is a valid reference
    /// </summary>
    /// <param name="reference">The string to check</param>
    /// <returns>A boolean indicating whether or not the specified value is a valid reference</returns>
    public static bool IsValidReference(string reference)
    {
        if (string.IsNullOrWhiteSpace(reference)) throw new ArgumentNullException(nameof(reference));
        var components = reference.Split(':', StringSplitOptions.RemoveEmptyEntries);
        if (components.Length < 1 || components.Length > 2) return false;
        var name = components[0];
        if(components.Length == 2)
        {
            var version = components[1];
            if (!SemVersion.TryParse(version, SemVersionStyles.Strict, out _)) return false;
        }
        return !string.IsNullOrWhiteSpace(name) && name.Length > 3 && name.Length <= 63 && name.IsAlphanumeric('-');
    }

    /// <summary>
    /// Exposes <see cref="WorkflowDsl"/>-related labels
    /// </summary>
    public static class Labels
    {

        /// <summary>
        /// Gets the label used to reference a specific version of a workflow DSL
        /// </summary>
        public const string Reference = SynapseConstants.Resources.LabelPrefix + "workflow-dsl";

    }

}
