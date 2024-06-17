﻿// Copyright © 2024-Present The Synapse Authors
//
// Licensed under the Apache License, Version 2.0 (the "License"),
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Neuroglia.Data.Infrastructure.ResourceOriented;

namespace Synapse.Resources;

/// <summary>
/// Represents the resource used to describe a workflow instance
/// </summary>
[DataContract]
public record WorkflowInstance
    : Resource<WorkflowInstanceSpec, WorkflowInstanceStatus>
{

    /// <summary>
    /// Gets the <see cref="WorkflowInstance"/>'s resource type
    /// </summary>
    public static readonly ResourceDefinitionInfo ResourceDefinition = new WorkflowInstanceResourceDefinition()!;

    /// <inheritdoc/>
    public WorkflowInstance() : base(ResourceDefinition) { }

    /// <inheritdoc/>
    public WorkflowInstance(ResourceMetadata metadata, WorkflowInstanceSpec spec) : base(ResourceDefinition, metadata, spec) { }

    /// <summary>
    /// Gets a value indicating whether the workflow is in an operative state, meaning it is pending, running, or suspended, and can potentially resume execution.
    /// </summary>
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public virtual bool IsOperative => this.Status?.Phase == TaskInstanceStatus.Pending || this.Status?.Phase == TaskInstanceStatus.Running || this.Status?.Phase == TaskInstanceStatus.Suspended;

}
