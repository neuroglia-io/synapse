using Hylo.Api.Http;
using Synapse.Resources;

namespace Synapse.Api.Core.Http.Controllers;

/// <summary>
/// Represents the <see cref="ResourceApiController{TResource}"/> used to manage <see cref="Workflow"/>s
/// </summary>
[Route("api/core/v1/workflows")]
public class WorkflowsController
    : NamespacedResourceApiController<Workflow>
{

    /// <inheritdoc/>
    public WorkflowsController(IMediator mediator) : base(mediator) { }

}
