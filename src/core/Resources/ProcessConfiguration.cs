namespace Synapse.Resources;

/// <summary>
/// Represents an object used to configure a process
/// </summary>
[DataContract]
public record ProcessConfiguration
    : IExtensible
{

    /// <summary>
    /// Initializes a new <see cref="ProcessConfiguration"/>
    /// </summary>
    public ProcessConfiguration() { }

    /// <summary>
    /// Initializes a new <see cref="ProcessConfiguration"/>
    /// </summary>
    /// <param name="target">The name of the image/binary the configured process executes. Supports runtime expressions</param>
    /// <param name="entryPoint">The process's entry point. Supports runtime expressions</param>
    /// <param name="arguments">The process's arguments. Supports runtime expressions</param>
    /// <param name="environmentVariables"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ProcessConfiguration(string target, string? entryPoint = null, List<string>? arguments = null, IDictionary<string, string>? environmentVariables = null)
    {
        if(string.IsNullOrWhiteSpace(target)) throw new ArgumentNullException(nameof(target));
        this.Target = target;
        this.EntryPoint = entryPoint;
        this.Arguments = arguments;
        this.EnvironmentVariables = environmentVariables;
    }

    /// <summary>
    /// Gets/sets the name of the image/binary the configured process executes. Supports runtime expressions
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "target", IsRequired = true), JsonPropertyOrder(1), JsonPropertyName("target"), YamlMember(Order = 1, Alias = "target")]
    public virtual string Target { get; set; } = null!;

    /// <summary>
    /// Gets/sets the process's entry point. Supports runtime expressions
    /// </summary>
    [DataMember(Order = 2, Name = "entryPoint"), JsonPropertyOrder(2), JsonPropertyName("entryPoint"), YamlMember(Order = 2, Alias = "entryPoint")]
    public virtual string? EntryPoint { get; set; }

    /// <summary>
    /// Gets/sets the process's arguments. Supports runtime expressions
    /// </summary>
    [DataMember(Order = 3, Name = "arguments"), JsonPropertyOrder(3), JsonPropertyName("arguments"), YamlMember(Order = 3, Alias = "arguments")]
    public virtual List<string>? Arguments { get; set; }

    /// <summary>
    /// Gets/sets the process's environment variables. Supports runtime expressions
    /// </summary>
    [DataMember(Order = 4, Name = "environmentVariables"), JsonPropertyOrder(4), JsonPropertyName("environmentVariables"), YamlMember(Order = 4, Alias = "environmentVariables")]
    public virtual IDictionary<string, string>? EnvironmentVariables { get; set; }

    /// <inheritdoc/>
    [DataMember(Order = 999, Name = "extensionData"), JsonExtensionData]
    public virtual IDictionary<string, object>? ExtensionData { get; set; }

}
