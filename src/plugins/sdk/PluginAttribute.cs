using Synapse.Plugins.Sdk.Services;

namespace Synapse.Plugins.Sdk;

/// <summary>
/// Represents an attribute used to mark a class as a plugin
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PluginAttribute
    : Attribute
{

    /// <summary>
    /// Initializes a new <see cref="PluginAttribute"/>
    /// </summary>
    /// <param name="contractType">The plugin's contract type. Must be an interface</param>
    /// <param name="bootstrapperType">The plugin's bootstrapper type, if any. Must be an implementation of the <see cref="IPluginBootstrapper"/> interface</param>
    public PluginAttribute(Type contractType, Type? bootstrapperType = null)
    {
        if (contractType == null) throw new ArgumentNullException(nameof(contractType));
        if (contractType.IsInterface) throw new ArgumentException("The type must be an interface", nameof(contractType));
        if(bootstrapperType != null && !typeof(IPluginBootstrapper).IsAssignableFrom(bootstrapperType)) throw new ArgumentException($"The type must be a non-abstract, non-generic implementation of the {nameof(IPluginBootstrapper)} interface", nameof(contractType));
        this.ContractType = contractType;
        this.BootstrapperType = bootstrapperType;
    }

    /// <summary>
    /// Gets the plugin's contract type. Must be an interface
    /// </summary>
    public Type ContractType { get; } = null!;

    /// <summary>
    /// Gets the plugin's bootstrapper type, if any. Must be an implementation of the <see cref="IPluginBootstrapper"/> interface
    /// </summary>
    public Type? BootstrapperType { get; }

}
