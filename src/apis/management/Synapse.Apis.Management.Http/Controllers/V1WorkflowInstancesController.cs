﻿/*
 * Copyright © 2022-Present The Synapse Authors
 * <p>
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * http://www.apache.org/licenses/LICENSE-2.0
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

namespace Synapse.Apis.Management.Http.Controllers
{

    /// <summary>
    /// Represents the <see cref="ApiController"/> used to manage workflow definitions
    /// </summary>
    [Route("api/v1/workflow-instances")]
    public class V1WorkflowInstancesController
        : ApiController
    {

        /// <inheritdoc/>
        public V1WorkflowInstancesController(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper)
            : base(loggerFactory, mediator, mapper)
        {

        }

        /// <summary>
        /// Creates a new workflow instance
        /// </summary>
        /// <param name="command">An object that represents the command to execute</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
        /// <returns>A new <see cref="IActionResult"/></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Integration.Models.V1WorkflowInstance), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Create([FromBody] Integration.Commands.WorkflowInstances.V1CreateWorkflowInstanceCommand command, CancellationToken cancellationToken)
        {
            return this.Process(await this.Mediator.ExecuteAsync(this.Mapper.Map<Application.Commands.WorkflowInstances.V1CreateWorkflowInstanceCommand>(command), cancellationToken), (int)HttpStatusCode.Created);
        }

        /// <summary>
        /// Gets the workflow instance with the specified id.
        /// </summary>
        /// <param name="id">The id of the workflow instance to find</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
        /// <returns>A new <see cref="IActionResult"/></returns>
        [HttpGet("{id}"), EnableQuery]
        [ProducesResponseType(typeof(Integration.Models.V1WorkflowInstance), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            return this.Process(await this.Mediator.ExecuteAsync(new Application.Queries.Generic.V1FindByIdQuery<Integration.Models.V1WorkflowInstance, string>(id), cancellationToken));
        }

        /// <summary>
        /// Queries workflow instances
        /// <para>This endpoint supports ODATA.</para>
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
        /// <returns>A new <see cref="IActionResult"/></returns>
        [HttpGet, EnableQuery]
        [ProducesResponseType(typeof(IEnumerable<Integration.Models.V1WorkflowInstance>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            return this.Process(await this.Mediator.ExecuteAsync(new Application.Queries.Generic.V1ListQuery<Integration.Models.V1WorkflowInstance>(), cancellationToken));
        }

        /// <summary>
        /// Starts the workflow instance with the specified id
        /// </summary>
        /// <param name="id">The id of the workflow instance to start</param>
        /// <param name="at">The date and time at which to start the specified workflow instance</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
        /// <returns>A new <see cref="IActionResult"/></returns>
        [HttpPut("byid/{id}/start")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Start(string id, CancellationToken cancellationToken)
        {
            return this.Process(await this.Mediator.ExecuteAsync(new Application.Commands.WorkflowInstances.V1StartWorkflowInstanceCommand(id), cancellationToken));
        }

        /// <summary>
        /// Patches an existing workflow instance
        /// </summary>
        /// <param name="command">An object used to describe the command to execute</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
        /// <returns>A new <see cref="IActionResult"/></returns>
        [HttpPatch]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Patch([FromBody] Integration.Commands.Generic.V1PatchCommand command, CancellationToken cancellationToken)
        {
            return this.Process(await this.Mediator.ExecuteAsync(this.Mapper.Map<Application.Commands.Generic.V1PatchCommand<Domain.Models.V1WorkflowInstance, string>>(command), cancellationToken));
        }

        /// <summary>
        /// Deletes an existing workflow instance
        /// </summary>
        /// <param name="id">The id of the workflow definition to delete</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
        /// <returns>A new <see cref="IActionResult"/></returns>
        [HttpDelete("byid/{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            return this.Process(await this.Mediator.ExecuteAsync(new Application.Commands.Generic.V1DeleteCommand<Domain.Models.V1WorkflowInstance, string>(id), cancellationToken), (int)HttpStatusCode.Created);
        }

    }

}
