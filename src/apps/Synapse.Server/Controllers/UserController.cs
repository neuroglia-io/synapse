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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Synapse.Application.Configuration;
using Synapse.Server.Models;
using System.Security.Claims;

namespace Synapse.Server.Controllers
{

    /// <summary>
    /// Represents the <see cref="Controller"/> used to manage users
    /// </summary>
    [Route("api/v1/user")]
    public class UserController 
        : Controller
    {

        /// <summary>
        /// Initializes a new <see cref="UserController"/>
        /// </summary>
        /// <param name="options">The current <see cref="ApplicationOptions"/></param>
        public UserController(IOptions<ApplicationOptions> options)
        {
            this.Options = options.Value;
        }

        /// <summary>
        /// Gets the current <see cref="ApplicationOptions"/>
        /// </summary>
        protected ApplicationOptions Options { get; }

        /// <summary>
        /// Renders the login view
        /// </summary>
        /// <returns>A new <see cref="IActionResult"/></returns>
        [HttpGet("login"), AllowAnonymous]
        public IActionResult Login(string returnUrl = "~/")
        {
            if (this.Options.Authentication.Scheme == ApplicationAuthenticationScheme.OpenIdConnect)
                return this.RedirectToAction(nameof(ChallengeOpenIdConnect), new { returnUrl });
            else
                return this.View(new LoginViewModel() { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// Challenges the Open Id Connect scheme
        /// </summary>
        /// <param name="returnUrl">The url to redirect the user to after a successfull login attempt</param>
        /// <returns>A new <see cref="IActionResult"/></returns>
        [HttpGet("oidc/challenge"), AllowAnonymous]
        public IActionResult ChallengeOpenIdConnect(string returnUrl = "~/")
        {
            if (string.IsNullOrEmpty(returnUrl)) 
                returnUrl = "~/";
            var challenge = new OpenIdConnectChallengeProperties() { RedirectUri = returnUrl };
            return this.Challenge(challenge, OpenIdConnectDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Handles a login attempt
        /// </summary>
        /// <param name="model">The data used to log the user in</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
        /// <returns>A new <see cref="IActionResult"/></returns>
        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginInputModel model, CancellationToken cancellationToken)
        {
            if(!this.ModelState.IsValid)
                return this.BadRequest(this.ModelState);
            var claims = new List<Claim>() 
            { 
                new(JwtClaimTypes.AuthenticationMethod, CookieAuthenticationDefaults.AuthenticationScheme),
                new(JwtClaimTypes.Subject, "Admin"),
                new(JwtClaimTypes.PreferredUserName, "Admin"),
                new(JwtClaimTypes.Role, "admin"),
            };
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, JwtClaimTypes.AuthenticationMethod, JwtClaimTypes.PreferredUserName, JwtClaimTypes.Role));
            var authenticationProperties = new AuthenticationProperties();
            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user, authenticationProperties);
            return this.Redirect(model.ReturnUrl);
        }

    }

}
