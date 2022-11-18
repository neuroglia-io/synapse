/*
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

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Synapse.Apis.Management.Http.Generator
{

    [Generator]
    class HttpManagementApiSourceGenerator
        : ISourceGenerator
    {

        void ISourceGenerator.Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif 
            context.RegisterForSyntaxNotifications(() => new ManagementApiInterfaceSyntaxReceiver());
        }

        void ISourceGenerator.Execute(GeneratorExecutionContext context)
        {
            var syntaxReceiver = (ManagementApiInterfaceSyntaxReceiver)context.SyntaxReceiver;
            if (syntaxReceiver == null) return;
            foreach (var apiInterface in syntaxReceiver.ApiInterfaces)
            {
                var apiInterfaceName = apiInterface.Identifier.ToString();
                var controllerName = apiInterface.Identifier.ToString().Substring(1).Replace("Api", "Controller");
                var tag = apiInterfaceName.Replace("Api", string.Empty).ToLowerInvariant();
                var routePrefix = "api/v1";
                var route = $"{routePrefix}/{apiInterfaceName.Replace("Api", string.Empty).ToLowerInvariant()}";
                var sourceBuilder = new StringBuilder();
                sourceBuilder.Append(
$@"/*
 * Copyright © 2022-Present The Synapse Authors
 * <p>
 * Licensed under the Apache License, Version 2.0 (the ""License"");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * http://www.apache.org/licenses/LICENSE-2.0
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an ""AS IS"" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

namespace Synapse.Apis.Management.Http.Controllers;

[Tags(""{tag}"")]
[Route(""{route}"")]
public class {controllerName}
    : ApiController, {apiInterfaceName}
{{
    
    /// <inheritdoc/>
    public {controllerName}(ILoggerFactory loggerFactory, IMediator mediator, IMapper mapper) : base(loggerFactory, mediator, mapper) {{ }}

");
                foreach (var method in apiInterface.ChildNodes().OfType<MethodDeclarationSyntax>())
                {
                    var methodName = method.Identifier.ToString();
                    var parameters = string.Join(", ", method.ParameterList.ChildNodes().OfType<ParameterSyntax>().Select(n => $"{n.Type} {n.Identifier}"));
                    sourceBuilder.Append(
$@"
    /// <inheritdoc/>
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
    public async Task<IActionResult> {methodName}({parameters})
    {{
        
    }}");
                    sourceBuilder.Append(
$@"
}}
");
                }
                var source = sourceBuilder.ToString();
                context.AddSource(controllerName, source);
            }
        }

    }

}
