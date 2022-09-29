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

using System.ComponentModel.DataAnnotations;

namespace Synapse.Server.Models
{
    /// <summary>
    /// Holds the data used to perform a login attempt
    /// </summary>
    public class LoginInputModel
    {

        /// <summary>
        /// Gets/sets the username to login with
        /// </summary>
        [Required]
        public string Username { get; set; } = null!;

        /// <summary>
        /// Gets/sets the password to login with
        /// </summary>
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        /// <summary>
        /// Gets/sets a boolean indicating whether or not to remember the user, in case of a successfull login attempt
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// Gets/sets the url to redirect the user to in case of success
        /// </summary>
        public string ReturnUrl { get; set; } = "/";

    }

}
