using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Template.QrCode.Api.Models
{
    /// <summary>Personal data to encode directly into a QR image.</summary>
    public sealed record CreateQrCodeRequest
    {
        [Required, StringLength(128)]
        [JsonProperty("id")]
        public string Id { get; init; }

        [Required, StringLength(200)]
        [JsonProperty("name")]
        public string Name { get; init; }

        [Required, EmailAddress, StringLength(254)]
        [JsonProperty("email")]
        public string Email { get; init; }

        [Url, StringLength(1024)]
        [JsonProperty("photo", NullValueHandling = NullValueHandling.Ignore)]
        public string Photo { get; init; }
    }
}
