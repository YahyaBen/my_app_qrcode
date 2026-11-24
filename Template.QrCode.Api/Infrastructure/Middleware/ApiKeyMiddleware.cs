using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Template.QrCode.Api.Infrastructure.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private IConfiguration _Configuration;
        private readonly string _AllowedApiKeys;
        internal const string ApiKeyHeaderName = "X-API-KEY";

        private static readonly string[] ExcludedPaths =
        {
            "/api",
              "/api/swagger/*",
            "",
        };

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _Configuration = configuration;
            _AllowedApiKeys = _Configuration["AllowedApiKeys"];

        }

        public async Task InvokeAsync(HttpContext context)
        {

            if (ExcludedPaths.Any(p =>
                context.Request.Path.Equals(p, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
            {
                await WriteUnauthorizedResponse(context, "API Key is missing");
                return;
            }

            var apiKey = extractedApiKey.FirstOrDefault();
            if (string.IsNullOrEmpty(apiKey))
            {
                await WriteUnauthorizedResponse(context, "API Key is missing");
                return;
            }

            bool isValidKey = _AllowedApiKeys.Equals(apiKey);

            if (!isValidKey)
            {
                await WriteUnauthorizedResponse(context, "Invalid API Key");
                return;
            }

            await _next(context);
        }

        private static async Task WriteUnauthorizedResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";

            var response = new
            {
                valid = false,
                code = "UNAUTHORIZED",
                message
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
