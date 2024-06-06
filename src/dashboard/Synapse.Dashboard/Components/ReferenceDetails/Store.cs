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

using BlazorMonaco.Editor;
using Synapse.Dashboard.Components.WorkflowInstanceDetailsStateManagement;
using Synapse.Resources;

namespace Synapse.Dashboard.Components.ReferenceDetailsStateManagement;

/// <summary>
/// Represents the <see cref="ComponentStore{TState}" /> of a <see cref="ReferenceDetails"/>
/// </summary>
/// <param name="api">The service used interact with Synapse API</param>
/// <param name="jsRuntime">The service used from JS interop</param>
/// <param name="monacoEditorHelper">The service used ease Monaco Editor interactions</param>
/// <param name="serializer">The service used to serialize and deserialize JSON</param>
public class ReferenceDetailsStore(
    Api.Client.Services.ISynapseApiClient api,
    IJSRuntime jsRuntime,
    IMonacoEditorHelper monacoEditorHelper,
    IJsonSerializer serializer
)
    : ComponentStore<ReferenceDetailsState>(new())
{
    /// <summary>
    /// Gets an <see cref="IObservable{T}"/> used to observe <see cref="ReferenceDetailsState.Label"/> changes
    /// </summary>
    public IObservable<string?> Label => this.Select(state => state.Label).DistinctUntilChanged();

    /// <summary>
    /// Gets an <see cref="IObservable{T}"/> used to observe <see cref="ReferenceDetailsState.Reference"/> changes
    /// </summary>
    public IObservable<string?> Reference => this.Select(state => state.Reference).DistinctUntilChanged();

    /// <summary>
    /// Gets an <see cref="IObservable{T}"/> used to observe <see cref="ReferenceDetailsState.Document"/> changes
    /// </summary>
    public IObservable<string?> Document => this.Select(state => state.Document).DistinctUntilChanged();

    /// <summary>
    /// Gets an <see cref="IObservable{T}"/> used to observe <see cref="ReferenceDetailsState.Loading"/> changes
    /// </summary>
    public IObservable<bool> Loading => this.Select(state => state.Loading).DistinctUntilChanged();

    /// <summary>
    /// Gets an <see cref="IObservable{T}"/> used to observe <see cref="ReferenceDetailsState.TextEditor"/> changes
    /// </summary>
    public IObservable<StandaloneCodeEditor> TextEditor => this.Select(state => state.TextEditor).DistinctUntilChanged();

    /// <summary>
    /// Sets the state's <see cref="ReferenceDetailsState.Label"/>
    /// </summary>
    /// <param name="label">The new <see cref="ReferenceDetailsState.Label"/> value</param>
    public void SetLabel(string label)
    {
        this.Reduce(state => state with
        {
            Label = label
        });
    }

    /// <summary>
    /// Sets the state's <see cref="ReferenceDetailsState.Reference"/>
    /// </summary>
    /// <param name="reference">The new <see cref="ReferenceDetailsState.Reference"/> value</param>
    public void SetReference(string reference)
    {
        this.Reduce(state => state with
        {
            Reference = reference
        });
    }

    /// <summary>
    /// Sets the state's <see cref="ReferenceDetailsState.TextEditor"/>
    /// </summary>
    /// <param name="textEditor">The new <see cref="ReferenceDetailsState.TextEditor"/> value</param>
    public void SetTextEditor(StandaloneCodeEditor textEditor)
    {
        this.Reduce(state => state with
        {
            TextEditor = textEditor
        });
    }

    /// <summary>
    /// Loads the referenced documents
    /// </summary>
    /// <returns></returns>
    public async Task LoadReferencedDocument()
    {
        var reference = this.Get(state => state.Reference);
        var loading = this.Get(state => state.Loading);
        if (string.IsNullOrWhiteSpace(reference) && loading) return;
        var document = await api.WorkflowData.GetAsync(reference);
        var json = serializer.SerializeToText(document.Content);
        this.Reduce(state => state with { 
            Document = json,
            Loading = false
        });
        await this.SetTextEditorValueAsync();
    }

    /// <summary>
    /// Handles changed of the text editor's language
    /// </summary>
    /// <param name="language"></param>
    /// <returns></returns>
    public async Task ToggleTextBasedEditorLanguageAsync(string language)
    {
        var textEditor = this.Get(state => state.TextEditor);
        if (textEditor == null)
        {
            return;
        }
        var model = await textEditor.GetModel();
        var editorLanguage = await model.GetLanguageId();
        if (editorLanguage != language)
        {
            await this.SetTextBasedEditorLanguageAsync();
        }
    }

    /// <summary>
    /// Handles initialization of the text editor
    /// </summary>
    /// <returns></returns>
    public async Task OnTextBasedEditorInitAsync()
    {
        var reference = this.Get(state => state.Reference);
        var resourceUri = $"inmemory://{reference.ToLower()}";
        var textModel = await Global.GetModel(jsRuntime, resourceUri);
        if (textModel == null)
        {
            var document = this.Get(state => state.Document) ?? "";
            textModel = await Global.CreateModel(jsRuntime, document, monacoEditorHelper.PreferredLanguage, resourceUri);
            var textEditor = this.Get(state => state.TextEditor);
            await textEditor!.SetModel(textModel);
            this.Reduce(state => state with
            {
                TextModel = textModel
            });
        }
        else
        {
            await this.SetTextEditorValueAsync();
            await this.SetTextBasedEditorLanguageAsync();
        }
    }

    /// <summary>
    /// Changes the value of the text editor
    /// </summary>
    /// <returns></returns>
    async Task SetTextEditorValueAsync()
    {
        var textEditor = this.Get(state => state.TextEditor);
        if (textEditor != null)
        {
            var text = await textEditor.GetValue();
            var document = this.Get(state => state.Document) ?? "";
            if (text != document) await textEditor.SetValue(document);
        }
    }

    /// <summary>
    /// Sets the language of the text editor
    /// </summary>
    /// <returns></returns>
    public async Task SetTextBasedEditorLanguageAsync()
    {
        var textEditor = this.Get(state => state.TextEditor);
        var textModel = this.Get(state => state.TextModel);
        if (textEditor != null && textModel != null)
        {
            await Global.SetModelLanguage(jsRuntime, textModel, monacoEditorHelper.PreferredLanguage);
        }
    }

}