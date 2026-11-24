using System.Collections.Generic;

namespace Template.QrCode.Api.Infrastructure.Data.Configuration
{
    public class CorsSettings
    {
        public const string SectionName = "Cors";

        public List<string> AllowedOrigins { get; set; } = [];
        public bool AllowCredentials { get; set; } = false;
        public List<string> AllowedMethods { get; set; } = ["GET", "POST", "PUT", "DELETE", "OPTIONS"];
        public List<string> AllowedHeaders { get; set; } = ["Content-Type", "Authorization", "Accept-Language", "X-API-KEY"];
        public List<string> ExposedHeaders { get; set; } = ["X-Rate-Limit-Remaining", "X-Rate-Limit-Reset"];
    }
}
