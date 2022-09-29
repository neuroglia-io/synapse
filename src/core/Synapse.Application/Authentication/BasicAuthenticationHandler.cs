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

using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Synapse.Application.Authentication
{
    /// <summary>
    /// Represents the <see cref="AuthenticationHandler{TOptions}"/> used to handle 'Basic' scheme authentication 
    /// </summary>
    public class BasicAuthenticationHandler
        : AuthenticationHandler<BasicAuthenticationOptions>
    {

        /// <inheritdoc/>
        public BasicAuthenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {

        }

        /// <inheritdoc/>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!this.Request.Headers.TryGetValue("Authorization", out var headerValues))
                return AuthenticateResult.Fail("Authorization header is missing");
            var authorizationValues = headerValues.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (authorizationValues.Length != 2)
                return AuthenticateResult.Fail("Invalid authorization header format");
            var scheme = authorizationValues.First();
            var encoded = authorizationValues.Last();
            if (!scheme.Equals(BasicAuthenticationDefaults.AuthenticationScheme, StringComparison.InvariantCultureIgnoreCase))
                return AuthenticateResult.Fail($"Unsupported scheme '{scheme}'");
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
            var components = decoded.Split(':', StringSplitOptions.RemoveEmptyEntries);
            if (components.Length != 2)
                return AuthenticateResult.Fail("Invalid Basic authorization value format");
            var username = components.First();
            var password = components.Last();
            if (this.Options.Username != username
                || this.Options.Password != password)
                return AuthenticateResult.Fail("Invalid username and/or password");
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new(JwtClaimTypes.AuthenticationMethod, BasicAuthenticationDefaults.AuthenticationScheme),
                new(JwtClaimTypes.Subject, "Admin"),
                new(JwtClaimTypes.PreferredUserName, "Admin"),
                new(JwtClaimTypes.Role, "admin")
            }, JwtClaimTypes.AuthenticationMethod, JwtClaimTypes.PreferredUserName, JwtClaimTypes.Role));
            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(user, BasicAuthenticationDefaults.AuthenticationScheme)));
        }

    }

}
