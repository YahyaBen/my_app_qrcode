using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Template.QrCode.Api.Models.Schema
{
    public class Schema
    {
        //[Key]
        public string valueSetId { get; set; }
        public string valueSetDate { get; set; }
        public Dictionary<string, valueSetValues> valueSetValues { get; set; }



    }
    public class valueSetValues
    {

        public string display { get; set; }
        public string lang { get; set; }
        public string active { get; set; }
        public string system { get; set; }
        public string version { get; set; }

    }
}
