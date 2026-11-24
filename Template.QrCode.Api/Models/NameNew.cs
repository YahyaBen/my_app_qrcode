using Newtonsoft.Json;

namespace Template.QrCode.Api.Models
{
    public class NameNew
    {
        /// <summary>
        /// The surename or primary name(s) of the person addressed in the certificate
        /// </summary>
        [JsonProperty("fn", NullValueHandling = NullValueHandling.Ignore)]
        public string SurnameName { get; set; }

        /// <summary>
        /// The family name(s) of the person transliterated
        /// </summary>
        [JsonProperty("fnt")]
        public string SurameTransliterated { get; set; }

        /// <summary>
        /// The given name(s) of the person addressed in the certificate
        /// </summary>
        [JsonProperty("gn", NullValueHandling = NullValueHandling.Ignore)]
        public string GivenName { get; set; }

        /// <summary>
        /// The given name(s) of the person transliterated
        /// </summary>
        [JsonProperty("gnt", NullValueHandling = NullValueHandling.Ignore)]
        public string GivenNameTraslitaerated { get; set; }

        /// <summary>
        /// The given name(s) of the person transliterated
        /// </summary>
        [JsonProperty("Idn", NullValueHandling = NullValueHandling.Ignore)]
        public string Identifiant { get; set; }
    }
}