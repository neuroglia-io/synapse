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

using Synapse.Api.Client.Services;
using System.Reactive.Linq;

namespace Synapse.Dashboard.Components.ResourceManagement;

/// <summary>
/// Represents a <see cref="ComponentStore{TState}"/> used to manage Synapse <see cref="IResource"/>s of the specified type
/// </summary>
/// <typeparam name="TState">The type of the component's state</typeparam>
/// <typeparam name="TResource">The type of <see cref="IResource"/>s to manage</typeparam>
/// <param name="apiClient">The service used to interact with the Synapse API</param>
/// <param name="resourceEventHub">The <see cref="IResourceEventWatchHub"/> websocket service client</param>
public class NamespacedResourceManagementComponentStore<TState, TResource>(ISynapseApiClient apiClient, ResourceWatchEventHubClient resourceEventHub)
    : ResourceManagementComponentStoreBase<TState, TResource>(apiClient, resourceEventHub)
    where TResource : Resource, new()
    where TState : NamespacedResourceManagementComponentState<TResource>, new()
{

    /// <summary>
    /// Gets an <see cref="IObservable{T}"/> used to observe <see cref="Namespace"/>s
    /// </summary>
    public IObservable<EquatableList<Namespace>?> Namespaces => this.Select(s => s.Namespaces).DistinctUntilChanged();

    /// <summary>
    /// Gets an <see cref="IObservable{T}"/> used to observe current namespace
    /// </summary>
    public IObservable<string?> Namespace => this.Select(s => s.Namespace).DistinctUntilChanged();

    /// <inheritdoc/>
    protected override IObservable<ResourcesFilter> Filter => Observable.CombineLatest(
            this.Namespace,
            this.LabelSelectors,
            (@namespace, labelSelectors) => new ResourcesFilter()
            {
                Namespace = @namespace,
                LabelSelectors = labelSelectors
            }
        )
        .DistinctUntilChanged()
        .Do((filter) => Console.WriteLine("NamespacedResourceManagementComponentStore.Filter$ " + filter.ToString()));

    /// <summary>
    /// Sets the <see cref="NamespacedResourceManagementComponentState{TResource}.Namespace"/>
    /// </summary>
    /// <param name="namespace">The new namespace</param>
    public void SetNamespace(string? @namespace)
    {
        this.Reduce(state => state with
        {
            Namespace = @namespace
        });
    }

    /// <summary>
    /// Lists all available <see cref="Namespace"/>s
    /// </summary>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    public virtual async Task ListNamespaceAsync()
    {
        this.Reduce(state => state with
        {
            Loading = true
        });
        var namespaceList = new EquatableList<Namespace>(await (await this.ApiClient.Namespaces.ListAsync().ConfigureAwait(false)).ToListAsync().ConfigureAwait(false));
        this.Reduce(s => s with
        {
            Namespaces = namespaceList,
            Loading = false
        });
    }

    /// <inheritdoc/>
    public override async Task DeleteResourceAsync(TResource resource)
    {
        await this.ApiClient.ManageNamespaced<TResource>().DeleteAsync(resource.GetName(), resource.GetNamespace()!).ConfigureAwait(false);
    }

}
