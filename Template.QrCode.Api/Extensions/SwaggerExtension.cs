using Template.QrCode.Api.Infrastructure.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Template.QrCode.Api.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSchoolSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Template QR Code API", Version = "v1" });
                x.OperationFilter<QrCodeOperationFilter>();

                const string apiKeyScheme = "ApiKey";
                x.AddSecurityDefinition(apiKeyScheme, new OpenApiSecurityScheme
                {
                    Name = ApiKeyMiddleware.ApiKeyHeaderName,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Enter the API key configured for this server."
                });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = apiKeyScheme
                        }
                    }] = Array.Empty<string>()
                });

                // x.ExampleFilters();

                //x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    Description = "JWT Authorization header using the bearer scheme",
                //    Name = "Authorization",
                //    In = ParameterLocation.Header,
                //    Type = SecuritySchemeType.ApiKey
                //});
                //x.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {new OpenApiSecurityScheme{Reference = new OpenApiReference
                //    {
                //        Id = "Bearer",
                //        Type = ReferenceType.SecurityScheme
                //    }}, new List<string>()}
                //});

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //x.IncludeXmlComments(xmlPath);
            });
            return services;
        }
    }
}
