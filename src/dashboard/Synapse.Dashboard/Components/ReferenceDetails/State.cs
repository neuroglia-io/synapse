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

namespace Synapse.Dashboard.Components.ReferenceDetailsStateManagement;

/// <summary>
/// Represents the state of a <see cref="Synapse.Dashboard.Components.ReferenceDetails"/>
/// </summary>
public record ReferenceDetailsState
{

    /// <summary>
    /// Gets/sets the label to display
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Gets/sets the reference to load
    /// </summary>
    public string Reference { get; set; } = string.Empty;

    /// <summary>
    /// Gets/sets the referenced document
    /// </summary>
    public string Document { get; set; } = string.Empty;

    /// <summary>
    /// Gets/sets a boolean indicating the reference is being loaded
    /// </summary>
    public bool Loading { get; set; } = true;

    /// <summary>
    /// Gets/sets the <see cref="StandaloneCodeEditor"/> used to display the referenced document
    /// </summary>
    public StandaloneCodeEditor? TextEditor { get; set; } = null;

    /// <summary>
    /// Gets/sets the <see cref="TextModel"/> used by the <see cref="StandaloneCodeEditor"/>
    /// </summary>
    public TextModel? TextModel { get; set; } = null;
}
