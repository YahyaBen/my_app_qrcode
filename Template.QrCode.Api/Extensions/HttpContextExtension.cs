using Microsoft.Extensions.DependencyInjection;
using Template.QrCode.Api.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template.QrCode.Api.Extensions
{
    public static class HttpContextExtension
    {
        public static IServiceCollection AddHttpContextHelper(this IServiceCollection services)
        {
            services.AddSingleton<HttpContextHelper, HttpContextHelper>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}

