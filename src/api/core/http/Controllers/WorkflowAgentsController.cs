using Hylo.Api.Http;
using Synapse.Resources;

namespace Synapse.Api.Core.Http.Controllers;

/// <summary>
/// Represents the <see cref="ResourceApiController{TResource}"/> used to manage <see cref="WorkflowAgent"/>s
/// </summary>
[Route("api/core/v1/workflow-agents")]
public class WorkflowAgentsController
    : ClusterResourceApiController<WorkflowAgent>
{

    /// <inheritdoc/>
    public WorkflowAgentsController(IMediator mediator) : base(mediator) { }

}
