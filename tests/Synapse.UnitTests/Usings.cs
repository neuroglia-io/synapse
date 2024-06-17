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

global using Synapse;
global using Synapse.Resources;
global using Synapse.Runner.Services.Executors;
global using ServerlessWorkflow.Sdk;
global using ServerlessWorkflow.Sdk.Models;
global using ServerlessWorkflow.Sdk.Models.Calls;
global using ServerlessWorkflow.Sdk.Models.Tasks;
global using Synapse.UnitTests.Services;
global using FluentAssertions;
global using Microsoft.Extensions.DependencyInjection;
global using Neuroglia.Eventing.CloudEvents;
global using Neuroglia.Eventing.CloudEvents.Infrastructure.Services;
global using Neuroglia.Serialization;
global using Neuroglia.Serialization.Json;
