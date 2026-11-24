using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Template.QrCode.Api.Options
{

    public static class WebApplicationExtensions
    {
        public static WebApplication ConfigureSwagger(this WebApplication app)
        {
            var enableSwagger = app.Configuration.GetValue<bool>("EnableSwagger", false);

            if (enableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }

      
    }
}
