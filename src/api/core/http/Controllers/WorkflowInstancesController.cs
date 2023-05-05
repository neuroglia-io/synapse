using Hylo.Api.Http;
using Synapse.Resources;

namespace Synapse.Api.Core.Http.Controllers;

/// <summary>
/// Represents the <see cref="ResourceApiController{TResource}"/> used to manage <see cref="WorkflowInstance"/>s
/// </summary>
[Route("api/core/v1/workflow-instances")]
public class WorkflowInstancesController
    : ClusterResourceApiController<WorkflowInstance>
{

    /// <inheritdoc/>
    public WorkflowInstancesController(IMediator mediator) : base(mediator) { }

}
