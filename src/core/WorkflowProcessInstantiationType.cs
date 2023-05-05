using Hylo.Serialization.Json;
using Hylo.Serialization;
using System.ComponentModel;

namespace Synapse;

/// <summary>
/// Enumerates the different ways that a workflow can be instantiated
/// </summary>
[TypeConverter(typeof(StringEnumTypeConverter))]
[JsonConverter(typeof(JsonStringEnumConverterFactory))]
public enum WorkflowInstantiationType
{
    /// <summary>
    /// Indicates that the workflow instance has been manually created by a user
    /// </summary>
    [EnumMember(Value = "manual")]
    Manual = 1,
    /// <summary>
    /// Indicates that the workflow instance has been created by the workflow instance it is the child of
    /// </summary>
    [EnumMember(Value = "parent")]
    Parent = 2,
    /// <summary>
    /// Indicates that the workflow instance has been created in response to a trigger such as an event
    /// </summary>
    [EnumMember(Value = "trigger")]
    Trigger = 4,
    /// <summary>
    /// Indicates that the workflow instance has been created by a pre-defined schedule such as a CRON job
    /// </summary>
    [EnumMember(Value = "schedule")]
    Schedule = 8
}
