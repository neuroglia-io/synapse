// Copyright © 2024-Present The Synapse Authors
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

using Synapse.Resources;

namespace Synapse.Dashboard.Components.WorkflowInstanceDetailsStateManagement;

/// <summary>
/// Represents the <see cref="ComponentStore{TState}" /> of a <see cref="WorkflowInstanceDetails"/>
/// </summary>
public class WorkflowInstanceDetailsStore()
    : ComponentStore<WorkflowInstanceDetailsState>(new())
{

    /// <summary>
    /// Gets an <see cref="IObservable{T}"/> used to observe <see cref="WorkflowInstance"/> changes
    /// </summary>
    public IObservable<WorkflowInstance?> WorkflowInstance => this.Select(state => state.WorkflowInstance).DistinctUntilChanged();

    /// <summary>
    /// Sets the state's <see cref="WorkflowInstanceDetailsState.WorkflowInstance"/>
    /// </summary>
    /// <param name="workflowInstance">The new <see cref="WorkflowInstanceDetailsState.WorkflowInstance"/> value</param>
    public void SetWorkflowInstance(WorkflowInstance workflowInstance)
    {
        this.Reduce(state => state with
        {
            WorkflowInstance = workflowInstance
        });
    }
}