using Hylo.Serialization.Json;
using Hylo.Serialization;
using System.ComponentModel;

namespace Synapse;

/// <summary>
/// Enuemrates the possible phases a workflow instance can go through 
/// </summary>
[TypeConverter(typeof(StringEnumTypeConverter))]
[JsonConverter(typeof(JsonStringEnumConverterFactory))]
public enum WorkflowInstancePhase
{
    /// <summary>
    /// Indicates that the workflow instance is pending execution
    /// </summary>
    [EnumMember(Value = "pending")]
    Pending,
    /// <summary>
    /// Indicates that the workflow instance is being executed
    /// </summary>
    [EnumMember(Value = "running")]
    Running,
    /// <summary>
    /// Indicates that the execution of the workflow instance has been suspended
    /// </summary>
    [EnumMember(Value = "suspended")]
    Suspended,
    /// <summary>
    /// Indicates that the execution of the workflow instance faulted
    /// </summary>
    [EnumMember(Value = "faulted")]
    Faulted,
    /// <summary>
    /// Indicates that the execution of the workflow instance has been cancelled
    /// </summary>
    [EnumMember(Value = "cancelled")]
    Cancelled,
    /// <summary>
    /// Indicates that the execution of the workflow instance completed successfully
    /// </summary>
    [EnumMember(Value = "completed")]
    Completed
}