namespace Synapse.Resources;

/// <summary>
/// Represents an object used to configure a workflow instance
/// </summary>
[DataContract]
public record WorkflowInstanceSpec
{

    /// <summary>
    /// Initializes a new <see cref="WorkflowInstanceSpec"/>
    /// </summary>
    public WorkflowInstanceSpec() { }

    /// <summary>
    /// Initializes a new <see cref="WorkflowInstanceSpec"/>
    /// </summary>
    /// <param name="workflow">A reference to the workflow the instance belongs to. It must follow the format '{namespace}.{name}'. Namespace and name must following Hylo naming conventions</param>
    /// <param name="key">The workflow instance's unique key. Must contains a maximum of 63 alphanumeric characters or '-' characters</param>
    /// <param name="instantiationType">A value that indicates how the workflow instance has been instantiated</param>
    /// <param name="parent">A reference to the workflow instance the defined instance is the child of, if any. It must follow the format '{namespace}.{name}'. Namespace and name must following Hylo naming conventions</param>
    /// <param name="inputData">A reference to the instance's input data, if any. If set, must be a valid url following the format '{storageProvider}://{storageSpecificReference}' (ex: minio://workflows/pets/order/po-2023Q200069)</param>
    public WorkflowInstanceSpec(string workflow, string key, WorkflowInstantiationType instantiationType, string? parent = null, Uri? inputData = null)
    {
        if (string.IsNullOrWhiteSpace(workflow)) throw new ArgumentNullException(nameof(workflow));
        if (workflow.Length < 7 || workflow.Length > 126 || !workflow.IsAlphanumeric('-', '.') || workflow.Split('.').Length != 2) throw new ArgumentException($"The specified value '{workflow}' is not a valid workflow reference", nameof(workflow));
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
        if (key.Length < 3 || workflow.Length > 63 || !key.IsAlphanumeric('-')) throw new ArgumentException($"The specified value '{key}' is not a valid workflow process key", nameof(workflow));
        if (!string.IsNullOrWhiteSpace(parent) && (parent.Length < 7 || parent.Length > 126 || !parent.IsAlphanumeric('-', '.') || parent.Split('.').Length != 2)) throw new ArgumentException($"The specified value '{workflow}' is not a valid workflow process reference", nameof(parent));
        this.Workflow = workflow;
        this.Key = key;
        this.InstantiationType = instantiationType;
        this.Parent = parent;
        this.InputData = inputData;
    }

    /// <summary>
    /// Gets/sets a reference to the instance's workflow. It must follow the format '{namespace}.{name}'. Namespace and name must following Hylo naming conventions
    /// </summary>
    [Required, MinLength(7), MaxLength(126)]
    [DataMember(Order = 1, Name = "workflow", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("workflow"), YamlMember(Order = 1, Alias = "workflow")]
    public virtual WorkflowVersionReference Workflow { get; set; } = null!;

    /// <summary>
    /// Gets/sets the workflow instance's unique key. Must contains a maximum of 63 alphanumeric characters or '-' characters
    /// </summary>
    [Required, MinLength(3), MaxLength(63)]
    [DataMember(Order = 2, Name = "key", IsRequired = true), JsonPropertyOrder(2), JsonPropertyName("key"), YamlMember(Order = 2, Alias = "key")]
    public virtual string Key { get; set; } = null!;

    /// <summary>
    /// Gets/sets a value that indicates how the workflow instance has been instantiated
    /// </summary>
    [Required, MinLength(3), MaxLength(63)]
    [DataMember(Order = 3, Name = "instantiationType", IsRequired = true), JsonPropertyOrder(3), JsonPropertyName("instantiationType"), YamlMember(Order = 3, Alias = "instantiationType")]
    public virtual WorkflowInstantiationType InstantiationType { get; set; }

    /// <summary>
    /// Gets/sets a reference to the workflow instance the defined instance is the child of, if any. It must follow the format '{namespace}.{name}'. Namespace and name must following Hylo naming conventions
    /// </summary>
    [Required, MinLength(7), MaxLength(126)]
    [DataMember(Order = 4, Name = "parent", IsRequired = true), JsonPropertyOrder(4), JsonPropertyName("parent"), YamlMember(Order = 4, Alias = "parent")]
    public virtual string? Parent { get; set; }

    /// <summary>
    /// Gets/sets a reference to the instance's input data, if any. If set, must be a valid url following the format '{storageProvider}://{storageSpecificReference}' (ex: minio://workflows/pets/order/po-2023Q200069)
    /// </summary>
    [DataMember(Order = 5, Name = "inputData", IsRequired = true), JsonPropertyOrder(5), JsonPropertyName("inputData"), YamlMember(Order = 5, Alias = "inputData")]
    public virtual Uri? InputData { get; set; }

}
