using Hylo.Serialization.Json;
using Hylo.Serialization;
using System.ComponentModel;

namespace Synapse;

/// <summary>
/// Enumerates the different ways that a process can be instantiated
/// </summary>
[TypeConverter(typeof(StringEnumTypeConverter))]
[JsonConverter(typeof(JsonStringEnumConverterFactory))]
public enum WorkflowProcessInstantiationType
{
    /// <summary>
    /// Indicates that the process has been manually created by a user
    /// </summary>
    [EnumMember(Value = "manual")]
    Manual = 1,
    /// <summary>
    /// Indicates that the process has been created by the process it is the child of
    /// </summary>
    [EnumMember(Value = "parent")]
    Parent = 2,
    /// <summary>
    /// Indicates that the process has been created in response to a trigger such as an event
    /// </summary>
    [EnumMember(Value = "trigger")]
    Trigger = 4,
    /// <summary>
    /// Indicates that the process has been created by a pre-defined schedule such as a CRON job
    /// </summary>
    [EnumMember(Value = "schedule")]
    Schedule = 8
}

/// <summary>
/// Exposes constants about Synapse
/// </summary>
public static class SynapseConstants
{

    /// <summary>
    /// Exposes constants about Synapse resources
    /// </summary>
    public static class Resources
    {

        /// <summary>
        /// Gets the API groups for all Synapse resources
        /// </summary>
        public const string ApiGroup = "synapse-wfms.io";

        /// <summary>
        /// Gets the prefix for all Synapse labels
        /// </summary>
        public const string LabelPrefix = "synapse-wfms.io/";

    }

}