using Microsoft.Extensions.DependencyInjection;

namespace Synapse.Plugins.Sdk.Services;

/// <summary>
/// Defines the fundamentals of a service used to bootstrap a plugin
/// </summary>
public interface IPluginBootstrapper
{

    /// <summary>
    /// Configures the plugin's services
    /// </summary>
    /// <param name="services">The plugin's <see cref="IServiceCollection"/></param>
    void ConfigureServices(IServiceCollection services);

}
