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

using AutoMapper;

namespace Synapse.Application.Configuration
{

    /// <summary>
    /// Defines the fundamentals of a service used to build a Synapse application
    /// </summary>
    public interface IApplicationBuilder
    {

        /// <summary>
        /// Gets the application's <see cref="IConfiguration"/>
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the application's <see cref="IServiceCollection"/>
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Gets the current <see cref="ApplicationOptions"/>
        /// </summary>
        ApplicationOptions Options { get; }

        /// <summary>
        /// Configures the <see cref="IApplicationBuilder"/> to use the specified <see cref="Profile"/>
        /// </summary>
        /// <typeparam name="TProfile">The type of the <see cref="Profile"/> to use</typeparam>
        /// <returns>The configured <see cref="IApplicationBuilder"/></returns>
        IApplicationBuilder AddMappingProfile<TProfile>()
            where TProfile : Profile;

    }

}
