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

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Synapse.Application.Authentication;

namespace Synapse.Application.Configuration
{
    /// <summary>
    /// Represents the options used to configure authentication on the Synapse application
    /// </summary>
    public class AuthenticationOptions
    {

        /// <summary>
        /// Gets the application's authentication scheme
        /// </summary>
        public virtual ApplicationAuthenticationScheme Scheme => this.OpenIdConnect == null ? ApplicationAuthenticationScheme.Basic : ApplicationAuthenticationScheme.OpenIdConnect;

        /// <summary>
        /// Gets/sets the options used to configure the application's OpenIdConnect authentication scheme
        /// </summary>
        public virtual OpenIdConnectOptions? OpenIdConnect { get; set; }

        /// <summary>
        /// Gets/sets the options used to configure the application's Basic authentication scheme
        /// </summary>
        public virtual BasicAuthenticationOptions? Basic { get; set; }

    }

}
