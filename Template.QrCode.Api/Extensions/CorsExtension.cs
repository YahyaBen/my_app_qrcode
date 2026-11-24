using Template.QrCode.Api.Infrastructure.Data.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template.QrCode.Api.Extensions
{
    public static class CorsExtension
    {
        public static IServiceCollection AddCorsConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                    //.AllowCredentials();

                });
            });
            return services;
        }
        public static IServiceCollection AddCorsServices(this IServiceCollection services, IConfiguration configuration)
        {
            var corsSettings = configuration.GetSection(CorsSettings.SectionName).Get<CorsSettings>() ?? new CorsSettings();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowCORS", policy =>
                {
                    if (corsSettings.AllowedOrigins.Contains("*"))
                    {
                        policy.AllowAnyOrigin()
                              .WithMethods([.. corsSettings.AllowedMethods])
                              .WithHeaders([.. corsSettings.AllowedHeaders])
                              .WithExposedHeaders([.. corsSettings.ExposedHeaders]);
                    }
                    else
                    {
                        policy.WithOrigins([.. corsSettings.AllowedOrigins])
                              .WithMethods([.. corsSettings.AllowedMethods])
                              .WithHeaders([.. corsSettings.AllowedHeaders])
                              .WithExposedHeaders([.. corsSettings.ExposedHeaders]);

                        if (corsSettings.AllowCredentials)
                        {
                            policy.AllowCredentials();
                        }
                    }
                });
            });

            return services;
        }
    }
}
