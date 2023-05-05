using Hylo.Serialization.Json;
using Hylo.Serialization;
using System.ComponentModel;

namespace Synapse;

/// <summary>
/// Enuemrates the possible phases a process can go through 
/// </summary>
[TypeConverter(typeof(StringEnumTypeConverter))]
[JsonConverter(typeof(JsonStringEnumConverterFactory))]
public enum ProcessPhase
{
    /// <summary>
    /// Indicates that the process has been created and is pending execution
    /// </summary>
    [EnumMember(Value = "pending")]
    Pending,
    /// <summary>
    /// Indicates that the process is running
    /// </summary>
    [EnumMember(Value = "running")]
    Running,
    /// <summary>
    /// Indicates that the process is not responding
    /// </summary>
    [EnumMember(Value = "not-responding")]
    NotResponding,
    /// <summary>
    /// Indicates that the process has exited
    /// </summary>
    [EnumMember(Value = "exited")]
    Exited
}