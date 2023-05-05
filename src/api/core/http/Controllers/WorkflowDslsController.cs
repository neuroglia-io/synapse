using Hylo.Api.Http;
using Synapse.Resources;

namespace Synapse.Api.Core.Http.Controllers;

/// <summary>
/// Represents the <see cref="ResourceApiController{TResource}"/> used to manage <see cref="WorkflowDsl"/>s
/// </summary>
[Route("api/core/v1/workflow-dsls")]
public class WorkflowDslsController
    : ClusterResourceApiController<WorkflowDsl>
{

    /// <inheritdoc/>
    public WorkflowDslsController(IMediator mediator) : base(mediator) { }

}
