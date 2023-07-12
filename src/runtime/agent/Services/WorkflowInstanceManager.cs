using Hylo.Infrastructure.Configuration;
using Json.Patch;
using Microsoft.Extensions.Options;
using Synapse.Runtime.ProcessManagement.Services;
using System.Net;

namespace Synapse.Runtime.Agent.Services;

/// <summary>
/// Represents a service used to manage <see cref="WorkflowInstance"/>s
/// </summary>
public class WorkflowInstanceManager
    : ResourceController<WorkflowInstance>
{

    /// <inheritdoc/>
    public WorkflowInstanceManager(ILoggerFactory loggerFactory, IOptions<ResourceControllerOptions<WorkflowInstance>> controllerOptions, IRepository repository, IProcessManager processManager) 
        : base(loggerFactory, controllerOptions, repository)
    {
        this.ProcessManager = processManager;
    }

    /// <summary>
    /// Gets the current <see cref="IProcessManager"/>
    /// </summary>
    protected IProcessManager ProcessManager { get; }

    /// <summary>
    /// Gets the service used to monitor the resource that defines the running agent
    /// </summary>
    protected IResourceMonitor<WorkflowAgent> Agent { get; private set; } = null!;

    /// <summary>
    /// Gets a <see cref="ConcurrentDictionary{TKey, TValue}"/> used to maintain an in-memory mapping of running processes per workflow instance id
    /// </summary>
    protected ConcurrentDictionary<string, IProcess> Processes { get; } = new();

    /// <inheritdoc/>
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await base.StartAsync(this.CancellationTokenSource.Token).ConfigureAwait(false);
        this.Agent = await this.Repository.MonitorAsync<WorkflowAgent>(Environment.GetEnvironmentVariable("SYNAPSE_AGENT_NAME") ?? AppDomain.CurrentDomain.FriendlyName, Environment.GetEnvironmentVariable("SYNAPSE_AGENT_NAMESPACE") ?? Namespace.DefaultNamespaceName, cancellationToken: cancellationToken).ConfigureAwait(false);
        foreach(var instance in this.Resources.Values.ToList())
        {
            if (!await this.TryRunProcessAsync(instance, cancellationToken).ConfigureAwait(false)) continue;
        }
    }

    protected virtual async Task<bool> TryClaimInstanceAsync(WorkflowInstance instance, CancellationToken cancellationToken = default)
    {
        if(instance == null) throw new ArgumentNullException(nameof(instance));
        do
        {
            try
            {
                if (instance.Metadata.Labels == null) instance.Metadata.Labels = new Dictionary<string, string>();
                if (instance.Metadata.Labels.TryGetValue(WorkflowInstance.Labels.Agent, out var agentQualifiedName)) return agentQualifiedName == instance.GetQualifiedName();
                instance.Metadata.Labels.Add(WorkflowInstance.Labels.Agent, this.Agent.Resource.GetQualifiedName());
                instance = await this.Repository.ReplaceAsync(instance, cancellationToken: cancellationToken).ConfigureAwait(false);
                return true;
            }
            catch(HyloException ex) when (ex.Problem.Status == (int)HttpStatusCode.Conflict)
            {
                instance = (await this.Repository.GetAsync<WorkflowInstance>(instance.GetName(), instance.GetNamespace(), cancellationToken).ConfigureAwait(false))!;
            }
        }
        while (true);
    }

    /// <summary>
    /// Attempts to run the specified <see cref="WorkflowInstance"/>
    /// </summary>
    /// <param name="instance">The <see cref="WorkflowInstance"/> to run</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A boolean indicating whether or not the <see cref="WorkflowInstance"/> could be run</returns>
    protected virtual async Task<bool> TryRunProcessAsync(WorkflowInstance instance, CancellationToken cancellationToken = default)
    {
        if (instance == null) throw new ArgumentNullException(nameof(instance));
        var instanceToRun = instance.Clone()!;
        var matchingRule = this.Agent.Resource.Spec.ProcessRules.FirstOrDefault(r => true/*r.AppliesTo(instance.Spec.Workflow)*/);
        var patchOperations = new List<PatchOperation>(2);
        IProcess? process;
        if (matchingRule == null)
        {
            this.Logger.LogDebug("Failed to find a process rule matching workflow instance '{workflowInstance}'", instanceToRun.GetQualifiedName());
            return false;
        }
        if(instanceToRun.Status == null)
        {
            if (!await this.TryClaimInstanceAsync(instanceToRun, cancellationToken).ConfigureAwait(false))
            {
                this.Logger.LogDebug("Workflow instance '{workflowInstance}' has already been claimed by agent '{agentId}'", instanceToRun.GetQualifiedName(), string.IsNullOrWhiteSpace(instanceToRun.Status?.Agent)? "unknown" : instanceToRun.Status.Agent);
                return false;
            }
            instanceToRun.Status = new(this.Agent.Resource.GetQualifiedName());
        }
        else if (instanceToRun.Status.Phase == WorkflowInstancePhase.Pending)
        {
            if (this.Processes.TryGetValue(instance.GetQualifiedName(), out process))
            {
                this.Logger.LogDebug("Workflow instance '{workflowInstance}' is already being processed (process id: {processId})", instanceToRun.GetQualifiedName(), process.Id);
                return false;
            }
        }
        else if(instanceToRun.Status.Phase == WorkflowInstancePhase.Running)
        {
            if (this.Processes.TryGetValue(instance.GetQualifiedName(), out process)) return false;
            this.Logger.LogDebug("Workflow instance '{workflowInstance}' is marked as running but does not have an active process. Patching phase...", instanceToRun.GetQualifiedName());
            instanceToRun.Status.Runtimes?.Where(r => r.Agent == this.Agent.Resource.GetQualifiedName() && !r.EndedAt.HasValue).ToList().ForEach(r => r.EndedAt = DateTimeOffset.Now);
            instanceToRun = await this.Repository.ReplaceStatusAsync(instance, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        else
        {
            return false;
        }
        process = await this.ProcessManager.CreateProcessAsync(matchingRule.Process, cancellationToken).ConfigureAwait(false);
        if (!this.Processes.TryAdd(instance.GetQualifiedName(), process))
        {
            this.Logger.LogDebug("Workflow instance '{workflowInstance}' is already being processed", instanceToRun.GetQualifiedName());
            try
            {
                await process.DisposeAsync().ConfigureAwait(false);
            }
            catch { }
            return false;
        }
        var runtime = new WorkflowInstanceRuntime(this.Agent.Resource.GetQualifiedName(), process.Id);
        if (instanceToRun.Status!.Runtimes == null) instanceToRun.Status.Runtimes = new();
        instanceToRun.Status.Runtimes.Add(runtime);
        instanceToRun = await this.Repository.ReplaceStatusAsync(instanceToRun, cancellationToken: this.CancellationTokenSource.Token).ConfigureAwait(false);
        if (process.Phase == ProcessPhase.Pending) await process.StartAsync(this.CancellationTokenSource.Token).ConfigureAwait(false);
        this.Logger.LogInformation("Successfully started process '{processId}' to execute workflow instance '{workflowInstance}'", process.Id, instance.GetQualifiedName());
        return true;
    }

    /// <inheritdoc/>
    protected override Task OnResourceCreatedAsync(WorkflowInstance resource, CancellationToken cancellationToken = default)
    {
        return base.OnResourceCreatedAsync(resource, cancellationToken);
    }

    /// <inheritdoc/>
    protected override async Task OnResourceUpdatedAsync(WorkflowInstance instance, CancellationToken cancellationToken = default)
    {
        await base.OnResourceUpdatedAsync(instance, cancellationToken).ConfigureAwait(false);
        if (instance.Status == null) return;
        if (!instance.IsManagedBy(this.Agent.Resource))
        {
            this.Logger.LogDebug("Changes to workflow instance '{workflowInstance}' ignored because it belongs to another agent ('{agent}')", instance.GetQualifiedName(), instance.Status!.Agent);
            return;
        }
        this.Processes.TryGetValue(instance.GetQualifiedName(), out var process);
        switch (instance.Status.Phase)
        {
            case WorkflowInstancePhase.Pending:
                if (process == null) await this.TryRunProcessAsync(instance, cancellationToken).ConfigureAwait(false);
                break;
            case WorkflowInstancePhase.Running:
                if (process == null) await this.TryRunProcessAsync(instance, cancellationToken).ConfigureAwait(false);
                break;
            case WorkflowInstancePhase.Suspended:
            case WorkflowInstancePhase.Faulted:
            case WorkflowInstancePhase.Completed:
                if (process == null) break;
                try
                {
                    this.Processes.Remove(instance.GetQualifiedName(), out _);
                    await process.StopAsync(cancellationToken).ConfigureAwait(false);
                    await process.DisposeAsync().ConfigureAwait(false);
                    this.Logger.LogInformation("Terminated process '{processId}' as a result of workflow instance '{workflowInstance}' phasing to '{phase}'", process.Id, instance.GetQualifiedName(), EnumHelper.Stringify(instance.Status.Phase));
                }
                catch(Exception ex)
                {
                    this.Logger.LogError("An error occured while stopping the process '{processId}' of workflow instance '{workflowInstance}': {ex}", process.Id, instance.GetQualifiedName(), ex);
                }
                break;
        }
    }

    /// <inheritdoc/>
    protected override Task OnResourceDeletedAsync(WorkflowInstance resource, CancellationToken cancellationToken = default)
    {
        return base.OnResourceDeletedAsync(resource, cancellationToken);
    }

}