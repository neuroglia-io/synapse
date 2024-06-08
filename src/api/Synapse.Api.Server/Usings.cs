﻿// Copyright © 2024-Present Neuroglia SRL. All rights reserved.
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

global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.Extensions.Options;
global using Neuroglia;
global using Neuroglia.Serialization;
global using ServerlessWorkflow.Sdk;
global using Swashbuckle.AspNetCore.SwaggerUI;
global using Synapse;
global using Synapse.Api.Application;
global using Synapse.Api.Http;
global using Synapse.Api.Http.Hubs;
global using Synapse.Api.Server.Configuration;
global using System.Net.Mime;