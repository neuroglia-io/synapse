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
using System.Collections.Generic;
using System.Linq;

namespace Synapse.Apis.Management.Http.Generator
{
    internal class ManagementApiInterfaceSyntaxReceiver
        : ISyntaxReceiver
    {

        const string InterfaceName = "IManagementApiResource";

        internal List<InterfaceDeclarationSyntax> ApiInterfaces { get; } = new List<InterfaceDeclarationSyntax>();

        void ISyntaxReceiver.OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is InterfaceDeclarationSyntax interfaceSyntax)) return;
            var baseTypes = interfaceSyntax.BaseList?.DescendantNodes();
            if (baseTypes == null) return;
            var apiInterface = interfaceSyntax.BaseList?.DescendantNodes().OfType<SimpleBaseTypeSyntax>().FirstOrDefault(i => i.ToString() == InterfaceName);
            if (apiInterface == null) return;
            this.ApiInterfaces.Add(interfaceSyntax);
        }

    }

}
