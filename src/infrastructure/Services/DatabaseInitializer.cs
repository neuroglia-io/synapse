using Hylo;
using Hylo.Infrastructure.Services;
using Synapse.Resources;

namespace Synapse.Infrastructure.Services;

/// <summary>
/// Represents the service used to initialize the Synapse <see cref="IDatabase"/>
/// </summary>
public class DatabaseInitializer
    : Hylo.Infrastructure.Services.DatabaseInitializer
{

    /// <inheritdoc/>
    public DatabaseInitializer(IDatabase database) : base(database) { }

    /// <inheritdoc/>
    protected override async Task SeedAsync(CancellationToken cancellationToken)
    {
        await base.SeedAsync(cancellationToken);

        await this.Database.CreateResourceAsync(WorkflowDsl.ResourceDefinition, cancellationToken: cancellationToken).ConfigureAwait(false);
        await this.Database.CreateResourceAsync(WorkflowDslExtension.ResourceDefinition, cancellationToken: cancellationToken).ConfigureAwait(false);
        await this.Database.CreateResourceAsync(Workflow.ResourceDefinition, cancellationToken: cancellationToken).ConfigureAwait(false);
        await this.Database.CreateResourceAsync(WorkflowInstance.ResourceDefinition, cancellationToken: cancellationToken).ConfigureAwait(false);
        await this.Database.CreateResourceAsync(WorkflowAgent.ResourceDefinition, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

}
