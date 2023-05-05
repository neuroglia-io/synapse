using Synapse.Plugins.Management.Services;
using Synapse.Runtime.ProcessManagement.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPluginManager, PluginManager>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

var pluginsDirectory = Path.Combine(AppContext.BaseDirectory, "plugins");
var pluginManager = app.Services.GetRequiredService<IPluginManager>();
var plugin = await pluginManager.FindPluginsAsync<IProcessManager>().FirstOrDefaultAsync().ConfigureAwait(false) ?? throw new NullReferenceException($"Failed to find a valid workflow process manager plugin, required when using the '{nameof(PluginProcessManager)}'");
var process = await plugin.CreateProcessAsync(new() { Target = "jq" }).ConfigureAwait(false);
await process.StartAsync().ConfigureAwait(false);

await Task.Delay(2000);

app.Run();
