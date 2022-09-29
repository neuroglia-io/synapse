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

using CloudNative.CloudEvents.NewtonsoftJson;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Neuroglia.Data.Expressions;
using Neuroglia.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ServerlessWorkflow.Sdk;
using Synapse.Infrastructure;
using Synapse.Integration.Serialization.Converters;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Reactive.Subjects;
using System.Reflection;

namespace Synapse.Application.Configuration
{

    /// <summary>
    /// Represents the default implementation of the <see cref="IApplicationBuilder"/> interface
    /// </summary>
    public class ApplicationBuilder
        : IApplicationBuilder
    {

        /// <summary>
        /// Initializes a new <see cref="ApplicationBuilder"/>
        /// </summary>
        /// <param name="configuration">The current <see cref="IConfiguration"/></param>
        /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
        public ApplicationBuilder(IConfiguration configuration, IServiceCollection services)
        {
            this.Configuration = configuration;
            this.Services = services;
            this.Options = new();
            this.Configuration.Bind(this.Options);
        }

        /// <inheritdoc/>
        public IConfiguration Configuration { get; }

        /// <inheritdoc/>
        public IServiceCollection Services { get; }

        /// <inheritdoc/>
        public ApplicationOptions Options { get; }

        /// <summary>
        /// Gets a <see cref="List{T}"/> containing the assemblies to scan to automatically register mapping-related services
        /// </summary>
        protected List<Assembly> MapperAssemblies { get; } = new() { typeof(ApplicationBuilder).Assembly };

        /// <inheritdoc/>
        public virtual IApplicationBuilder AddMappingProfile<TProfile>() 
            where TProfile : AutoMapper.Profile
        {
            var assembly = typeof(TProfile).Assembly;
            if(!this.MapperAssemblies.Contains(assembly))
                this.MapperAssemblies.Add(assembly);
            return this;
        }

        /// <inheritdoc/>
        public virtual void Build()
        {
            
            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = new NonPublicSetterContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() { ProcessDictionaryKeys = false, OverrideSpecifiedNames = false, ProcessExtensionDataNames = false } },
                    Converters = new[] { new FilteredExpandoObjectConverter() },
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore
                };
                return settings;
            };

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            var writeModelTypes = TypeCacheUtil.FindFilteredTypes("syn:models-write", t => t.IsClass && !t.IsAbstract && typeof(IAggregateRoot).IsAssignableFrom(t), typeof(V1Workflow).Assembly).ToList();
            var readModelTypes = writeModelTypes
                .Where(t => t.TryGetCustomAttribute<DataTransferObjectTypeAttribute>(out _))
                .Select(t => t.GetCustomAttribute<DataTransferObjectTypeAttribute>()!.Type)
                .ToList();
            readModelTypes.AddRange(TypeCacheUtil.FindFilteredTypes("syn:models-read", t => t.IsClass && !t.IsAbstract && t.TryGetCustomAttribute<ReadModelAttribute>(out _)));
            readModelTypes = readModelTypes.Distinct().ToList();

            this.Services.AddSingleton(Microsoft.Extensions.Options.Options.Create(this.Options));
            this.Services.AddLogging();
            this.Services.AddMediator(builder =>
            {
                builder.ScanAssembly(typeof(ApplicationBuilder).Assembly);
                builder.UseDefaultPipelineBehavior(typeof(DomainExceptionHandlingMiddleware<,>));
                builder.UseDefaultPipelineBehavior(typeof(FluentValidationMiddleware<,>));
            });
            this.Services.AddGenericQueryHandlers();
            this.Services.AddGenericCommandHandlers();
            this.Services.AddMapper(this.MapperAssemblies.ToArray());
            this.Services.AddSingleton<IJsonPatchMetadataProvider, JsonPatchMetadataProvider>();
            this.Services.AddScoped<IObjectAdapter, AggregateObjectAdapter>();
            this.Services.AddTransient<IEdmModelBuilder, EdmModelBuilder>();
            this.Services.AddTransient<IODataQueryOptionsParser, ODataQueryOptionsParser>();
            this.Services.AddSingleton<WorkflowProcessManager>();
            this.Services.AddSingleton<IWorkflowProcessManager>(provider => provider.GetRequiredService<WorkflowProcessManager>());
            this.Services.AddSingleton<IHostedService>(provider => provider.GetRequiredService<WorkflowProcessManager>());
            this.Services.AddSingleton<IWorkflowRuntimeProxyFactory, WorkflowRuntimeProxyFactory>();
            this.Services.AddSingleton<IWorkflowRuntimeProxyManager, WorkflowRuntimeProxyManager>();
            this.Services.AddSingleton<ICronJobScheduler, CronJobScheduler>();
            this.Services.AddHostedService<WorkflowScheduler>();
            this.Services.AddHostedService<WorkflowDefinitionFileMonitor>();
            this.Services.AddTransient(provider => provider.GetRequiredService<IEdmModelBuilder>().Build());
            this.Services.AddNewtonsoftJsonSerializer(options =>
            {
                options.ContractResolver = new NonPublicSetterContractResolver() { NamingStrategy = new CamelCaseNamingStrategy() { ProcessDictionaryKeys = false, OverrideSpecifiedNames = false, ProcessExtensionDataNames = false } };
                options.NullValueHandling = NullValueHandling.Ignore;
                options.DefaultValueHandling = DefaultValueHandling.Ignore;
                options.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                options.Converters.Add(new AbstractClassConverterFactory());
                options.Converters.Add(new FilteredExpandoObjectConverter());
            });
            this.Services.AddSingleton<CloudEventFormatter, JsonEventFormatter>();
            this.Services.AddCloudEventBus(builder =>
            {
                builder.WithBrokerUri(this.Options.CloudEvents.Sink.Uri);
            });
            this.Services.AddServerlessWorkflow();
            this.Services.AddSingleton<CloudEventCorrelator>();
            this.Services.AddSingleton<IHostedService>(provider => provider.GetRequiredService<CloudEventCorrelator>());
            this.Services.AddIntegrationEventBus((provider, e) =>
            {
                var stream = provider.GetRequiredService<ISubject<CloudEvent>>();
                stream.OnNext(e);
                return Task.CompletedTask;
            });
            this.Services.AddIntegrationEventBus(async (provider, e) =>
            {
                await provider.GetRequiredService<ICloudEventBus>().PublishAsync(e);
            });
            this.Services.AddAuthorization(options =>
            {
                var schemes = new List<string>(2);
                switch (this.Options.Authentication.Scheme)
                {
                    case ApplicationAuthenticationScheme.Basic:
                        schemes.Add(BasicAuthenticationDefaults.AuthenticationScheme);
                        break;
                    case ApplicationAuthenticationScheme.OpenIdConnect:
                        schemes.Add(OpenIdConnectDefaults.AuthenticationScheme);
                        break;
                    default:
                        throw new NotSupportedException($"The specified {nameof(ApplicationAuthenticationScheme)} '{this.Options.Authentication.Scheme}' is not supported");
                }
                schemes.Add(CookieAuthenticationDefaults.AuthenticationScheme);
                options.DefaultPolicy = new AuthorizationPolicyBuilder(schemes.ToArray())
                    .RequireAuthenticatedUser()    
                    .Build();
               
            });
            var authenticationBuilder = this.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ClaimsIssuer = "Synapse";
                    options.LoginPath = "/api/v1/user/login";
                    options.LogoutPath = "/api/v1/user/logout";
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });
            this.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.Secure = CookieSecurePolicy.Always;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });
            switch (this.Options.Authentication.Scheme)
            {
                case ApplicationAuthenticationScheme.Basic:
                    authenticationBuilder.AddBasic(options =>
                    {
                        this.Options.Authentication.Basic?.CopyTo(options);
                    });
                    break;
                case ApplicationAuthenticationScheme.OpenIdConnect:
                    authenticationBuilder.AddOpenIdConnect(options =>
                    {
                        this.Options.Authentication.OpenIdConnect?.CopyTo(options);
                        options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;
                        options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                        options.TokenValidationParameters.NameClaimType = JwtClaimTypes.Name;
                        options.TokenValidationParameters.RoleClaimType = JwtClaimTypes.Role;
                    });
                    break;
                default:
                    throw new NotSupportedException($"The specified {nameof(ApplicationAuthenticationScheme)} '{this.Options.Authentication.Scheme}' is not supported");
            }
            this.Services.AddHttpContextAccessor();
            this.Services.AddScoped<IUserAccessor, HttpContextUserAccessor>();
            this.Services.AddSingleton<IExpressionEvaluatorProvider, ExpressionEvaluatorProvider>();
            this.Services.AddSingleton<PluginManager>();
            this.Services.AddSingleton<IPluginManager>(provider => provider.GetRequiredService<PluginManager>());
            this.Services.AddSingleton<IHostedService>(provider => provider.GetRequiredService<PluginManager>());
            this.Services.AddScoped<IRepositoryFactory, PluginBasedRepositoryFactory>();
            this.Services.AddRepositories(writeModelTypes, ServiceLifetime.Scoped, ApplicationModelType.WriteModel);
            this.Services.AddRepositories(readModelTypes, ServiceLifetime.Scoped, ApplicationModelType.ReadModel);

            if (this.Options.SkipCertificateValidation)
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
                this.Services.ConfigureAll<HttpClientFactoryOptions>(options =>
                {
                    options.HttpMessageHandlerBuilderActions.Add(builder =>
                    {
                        builder.PrimaryHandler = new HttpClientHandler
                        {
                            ClientCertificateOptions = ClientCertificateOption.Manual,
                            ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
                        };
                    });
                });
            }
        }

    }

}
