using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Template.QrCode.Api.Models
{
    /// <summary>
    /// EU Digital Green Certificate v.1.3.0
    /// </summary>
    public partial class DgCertificateImage
    {
        /// <summary>
        /// Date of birth of the DCC holder.  
        /// Complete or partial date without time restricted to the range from 1900-01-01 to 2099-12-31.  
        /// Exactly 1 (one) non-empty field MUST be provided if the complete or partial date of birth is known.If the date of birth is not known even partially, the field MUST be set to an empty string "". This should match the information as provided on travel documents.
        /// One of the following ISO 8601 formats MUST be used if information on date of birth is available.Other options are not supported.
        /// YYYY-MM-DD
        /// YYYY-MM
        /// YYYY
        /// </summary>
        //[JsonProperty("dob")]
        //public string DateOfBirthString { get; set; }
        //[JsonProperty("identity")]
        //public string Identifiant { get; set; }
        //[JsonProperty("typeIden")]
        //public int? TypeIdentifiant { get; set; }
        //[JsonProperty("Genre")]

        //public int? Genre { get; set; }
        //[JsonProperty("Nationality")]
        //public string Nationality { get; set; }

        [JsonProperty("FanId")]

        public string FanId { get; set; }
        [JsonProperty("Photo")]
        public string Photo { get; set; }

        //[JsonProperty("Nom")]

        //public string Nom { get; set; }
        //[JsonProperty("Prenom")]
        //public string Prenom { get; set; }
        //[JsonProperty("IsMajeur")]
        //public bool? IsMajeur { get; set; }


    }



}