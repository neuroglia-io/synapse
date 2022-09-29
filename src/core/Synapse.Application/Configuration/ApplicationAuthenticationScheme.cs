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

using System.Runtime.Serialization;

namespace Synapse.Application.Configuration
{
    /// <summary>
    /// Enumerates all authentication schemes supported by Synapse
    /// </summary>
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.StringEnumConverterFactory))]
    public enum ApplicationAuthenticationScheme
    {
        /// <summary>
        /// Indicates the 'Basic' authenticiation scheme. Not recommended in production scenarios.
        /// </summary>
        [EnumMember(Value = "basic")]
        Basic,
        /// <summary>
        /// Indicates the 'OpenIdConnect' authenticiation scheme.
        /// </summary>
        [EnumMember(Value = "oidc")]
        OpenIdConnect
    }

}
