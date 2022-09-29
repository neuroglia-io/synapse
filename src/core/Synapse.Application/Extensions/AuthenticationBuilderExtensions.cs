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

using Microsoft.AspNetCore.Authentication;
using Synapse.Application.Authentication;

namespace Synapse
{
    /// <summary>
    /// Defines extensions for <see cref="AuthenticationBuilder"/>s
    /// </summary>
    public static class AuthenticationBuilderExtensions
    {

        /// <summary>
        /// Configures the application to use the Basic scheme
        /// </summary>
        /// <param name="authentication">The <see cref="AuthenticationBuilder"/> to configure</param>
        /// <param name="configureOptions">An <see cref="Action{T}"/> used to configure the <see cref="BasicAuthenticationOptions"/></param>
        /// <returns>The configured <see cref="AuthenticationBuilder"/></returns>
        public static AuthenticationBuilder AddBasic(this AuthenticationBuilder authentication, Action<BasicAuthenticationOptions> configureOptions)
        {
            return authentication.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(BasicAuthenticationDefaults.AuthenticationScheme, configureOptions);
        }

    }

}
