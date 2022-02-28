using DevIO.Api.Middeware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using static DevIO.Api.Configuration.SwaggerConfig.ConfigureSwaggerOptions;

namespace DevIO.Api.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerDefaultValues>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;

        }

        public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app)
        {
            //app.UseMiddleware<SwaggerAuthorizedMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevIO.Api v1"));
            return app;
        }

        #region Extras

        public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
        {
            
            private readonly IApiVersionDescriptionProvider _provider;

            public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

            public void Configure(SwaggerGenOptions options)
            {
                foreach (var item in _provider.ApiVersionDescriptions) options.SwaggerDoc(item.GroupName, CreateInfoForApiVersion(item));
            }

            public static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
            {
                var info = new OpenApiInfo
                {
                    Title = "API - DEVIO",
                    Version = description.ApiVersion.ToString(),
                    Description = "API de estudo",
                    Contact = new OpenApiContact() { Name = "Eder Sanuki", Email = "eder@sanuki.com.br" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
                };

                if (description.IsDeprecated) info.Description += "Nada a declarar!";

                return info;
            }

            public class SwaggerDefaultValues : IOperationFilter
            {
                public void Apply(OpenApiOperation operation, OperationFilterContext context)
                {
                    if (operation.Parameters == null) return;

                    foreach (var item in operation.Parameters)
                    {
                        var description = context.ApiDescription.ParameterDescriptions.First(p => p.Name.Equals(item.Name));

                        var routeInfo = description.RouteInfo;

                        operation.Deprecated = OpenApiOperation.DeprecatedDefault;

                        if (item.Description == null) item.Description = description.ModelMetadata?.Description;

                        if (routeInfo == null) continue;

                        if (item.In != ParameterLocation.Path && item.Schema.Default == null)
                            item.Schema.Default = new OpenApiString(routeInfo.DefaultValue.ToString());

                        item.Required |= !routeInfo.IsOptional;
                    }

                }
            }
        }
        #endregion
    }
}
