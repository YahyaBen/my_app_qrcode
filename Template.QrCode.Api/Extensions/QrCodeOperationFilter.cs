using Template.QrCode.Api.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Template.QrCode.Api.Extensions
{
    public sealed class QrCodeOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.MethodInfo.DeclaringType == typeof(QrCodeController))
            {
                // File results bypass MVC formatters, so describe their media type explicitly.
                operation.Responses["200"].Content["image/png"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema { Type = "string", Format = "binary" }
                };
            }
        }
    }
}
