using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Template.QrCode.Api.Models
{
    public class Personne
    {
        //public string identity { get; set; }
        //public string CIN { get; set; }
        //public string Nom { get; set; }
        //public string Prenom { get; set; }
        //public DateTime? DateNaissance { get; set; }
        //public int? Sexe { get; set; }
        //public string Nationalité { get; set; }
        public string FanId { get; set; }

        public string Photo { get; set; }
        //public bool? IsMajeur{ get; set; }

    }
}
