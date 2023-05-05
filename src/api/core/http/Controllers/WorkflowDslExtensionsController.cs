using Hylo.Api.Http;
using Synapse.Resources;

namespace Synapse.Api.Core.Http.Controllers;

/// <summary>
/// Represents the <see cref="ResourceApiController{TResource}"/> used to manage <see cref="WorkflowDslExtension"/>s
/// </summary>
[Route("api/core/v1/workflow-dsl-extensions")]
public class WorkflowDslExtensionsController
    : ClusterResourceApiController<WorkflowDslExtension>
{

    /// <inheritdoc/>
    public WorkflowDslExtensionsController(IMediator mediator) : base(mediator) { }

}
