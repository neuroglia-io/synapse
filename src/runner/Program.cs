using Hylo.Infrastructure;
using Hylo.Providers.FileSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Synapse.Runner.Services;
using System.Security.Claims;

using var host = new HostBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddLogging();
        services.AddHttpClient();
        services.AddHylo(context.Configuration, builder =>
        {
            builder.UseUserAccessor<ClaimsPrincipalAccessor>();
            builder.UseFileSystem();
        });
        services.AddHostedService<DatabaseInitializer>();
        services.AddHostedService<WorkflowProcessManager>();
    })
    .Build();

await host.RunAsync().ConfigureAwait(false);

public class DatabaseInitializer
    : BackgroundService
{

    public DatabaseInitializer(IRepository repository)
    {
        this.Repository = repository;
    }

    protected IRepository Repository { get; }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (await this.Repository.GetDefinitionAsync<Workflow>(cancellationToken).ConfigureAwait(false) != null) return;

        await this.Repository.AddAsync(WorkflowDsl.ResourceDefinition, cancellationToken: cancellationToken).ConfigureAwait(false);
        await this.Repository.AddAsync(WorkflowDslExtension.ResourceDefinition, cancellationToken: cancellationToken).ConfigureAwait(false);
        await this.Repository.AddAsync(Workflow.ResourceDefinition, cancellationToken: cancellationToken).ConfigureAwait(false);
        await this.Repository.AddAsync(WorkflowProcess.ResourceDefinition, cancellationToken: cancellationToken).ConfigureAwait(false);
        await this.Repository.AddAsync(WorkflowRunner.ResourceDefinition, cancellationToken: cancellationToken).ConfigureAwait(false);

        await this.Repository.AddAsync(new WorkflowRunner(new ResourceMetadata(Environment.GetEnvironmentVariable("SYNAPSE_RUNNER_NAME")!, Environment.GetEnvironmentVariable("SYNAPSE_RUNNER_NAMESPACE")!), new WorkflowRunnerSpec("resource-controller", new() { "serverless-workflow:0.8" }))).ConfigureAwait(false);//todo: urgent: remove
    }

}

public class ClaimsPrincipalAccessor
    : IUserAccessor
{

    /// <inheritdoc/>
    public ClaimsPrincipal? User => ClaimsPrincipal.Current;

}