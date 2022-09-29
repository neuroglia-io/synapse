﻿/*
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

namespace Synapse.Application.Authentication
{

    /// <summary>
    /// Represents the options used to configure the Basic authentication scheme
    /// </summary>
    public class BasicAuthenticationOptions
        : AuthenticationSchemeOptions
    {

        /// <summary>
        /// Initializes a new <see cref="BasicAuthenticationOptions"/>
        /// </summary>
        public BasicAuthenticationOptions()
        {
            this.ClaimsIssuer = "Synapse";
        }

        /// <summary>
        /// Gets/sets the username to use
        /// </summary>
        public virtual string Username { get; set; } = null!;

        /// <summary>
        /// Gets/sets the password to use
        /// </summary>
        public virtual string Password { get; set; } = null!;

    }

}
