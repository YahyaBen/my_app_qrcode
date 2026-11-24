using Template.QrCode.Api.ClassesDB;
using Template.QrCode.Api.Extensions;
using Template.QrCode.Api.Models;
using Template.QrCode.Api.Options;
using Template.QrCode.Api.Repos;
using Template.QrCode.Api.Helpers;
using Template.QrCode.Api.Infrastructure.Data.Configuration;
using Template.QrCode.Api.Infrastructure.Middleware;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json.Serialization;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Template.QrCode.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.Configure<CorsSettings>(Configuration.GetSection(CorsSettings.SectionName));
            services
                //.AddSchoolDbContext(Configuration)
                    .AddSchoolSwagger()
                   // .AddHttpContextHelper()
                   // .AddJwtToken(Configuration)
                    .AddCorsServices(Configuration)
                    ;
            services.AddSingleton<ICertificateService, CertificateService>();

            //string DbConnectionString = Environment.GetEnvironmentVariable("DefaultConnectionString") ?? Configuration.GetConnectionString("DefaultConnectionString");
            string DbConnectionString = Configuration.GetConnectionString("DefaultConnectionString"); 
            services.AddControllers().AddFluentValidation().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
               services.AddDbContext<SignatureDbContext>(options => options.UseSqlServer(DbConnectionString));
 
    


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            
            var enableSwagger =  Configuration.GetValue<bool>("EnableSwagger", false);
            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions); 
            if (enableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
                });
            }

            app.UseRouting();
            app.UseCors("AllowCORS");
            app.UseAuthorization();
            app.UseMiddleware<ApiKeyMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
         

    }
}
