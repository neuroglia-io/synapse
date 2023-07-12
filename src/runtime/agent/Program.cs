using Hylo.Infrastructure;
using Hylo.Providers.FileSystem;
using Synapse.Runtime.Agent.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Synapse.Plugins.Services.PluginManager>();
builder.Services.AddSingleton<Synapse.Plugins.Services.IPluginManager>(provider => provider.GetRequiredService<Synapse.Plugins.Services.PluginManager>());
builder.Services.AddHostedService(provider => provider.GetRequiredService<Synapse.Plugins.Services.PluginManager>());

builder.Services.AddLogging();
builder.Services.AddHttpClient();
builder.Services.AddHylo(builder.Configuration, builder =>
{
    builder.UseFileSystem();
    builder.UseDatabaseInitializer<DatabaseInitializer>();
});

builder.Services.AddHostedService<WorkflowInstanceManager>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();

app.Run();

public class DatabaseInitializer
    : Synapse.Infrastructure.Services.DatabaseInitializer
{

    /// <inheritdoc/>
    public DatabaseInitializer(IDatabase database) : base(database) { }

    /// <inheritdoc/>
    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        await base.SeedAsync(cancellationToken);

        await this.Database.CreateResourceAsync(new WorkflowAgent(new ResourceMetadata(Environment.GetEnvironmentVariable("SYNAPSE_AGENT_NAME")!, Environment.GetEnvironmentVariable("SYNAPSE_AGENT_NAMESPACE")!), new WorkflowAgentSpec(new WorkflowAgentProcessRule[] { new("resource-controller", new string[] { "serverless-workflow:0.8" }, new()) } )), cancellationToken: cancellationToken).ConfigureAwait(false);//todo: urgent: remove
    }

}