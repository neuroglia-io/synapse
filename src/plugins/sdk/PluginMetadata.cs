using Synapse.Plugins.Sdk;
using System.Reflection;

namespace Synapse.Plugins.Skd;

/// <summary>
/// Represents an object used to describe a plugin
/// </summary>
public class PluginMetadata
{

    /// <summary>
    /// Initializes a new <see cref="PluginMetadata"/>
    /// </summary>
    /// <param name="pluginType">The plugin's type</param>
    public PluginMetadata(Type pluginType)
    {
        if(pluginType == null) throw new ArgumentNullException(nameof(pluginType));
        this.AssemblyFilePath = pluginType.Assembly.Location;
        this.AssemblyName = pluginType.Assembly.GetName().FullName;
        this.TypeName = pluginType.FullName!;
        this.BootstrapperTypeName = ((Type?)pluginType.GetCustomAttributesData().FirstOrDefault(a => a.AttributeType.FullName == typeof(PluginAttribute).FullName)?.ConstructorArguments[1].Value)?.FullName;
    }

    /// <summary>
    /// Gets the path to the plugin assembly file
    /// </summary>
    public string AssemblyFilePath { get; set; }

    /// <summary>
    /// gets the name of the plugin assembly
    /// </summary>
    public string AssemblyName { get; set; }

    /// <summary>
    /// Gets the name of the plugin type
    /// </summary>
    public string TypeName { get; set; }

    /// <summary>
    /// Gets the name of the plugin's bootstrapper type, if any
    /// </summary>
    public string? BootstrapperTypeName { get; }

}
