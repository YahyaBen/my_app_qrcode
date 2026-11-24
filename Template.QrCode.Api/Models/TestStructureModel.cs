using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Template.QrCode.Api.Models
{
    public class TestStructureModel
    {
     
        public string ID { get; set; }
        public DateTime TimeStamps { get; set; }
        public string Country { get; set; }
        public string SHA256FingerPrint { get; set; }
        public string CertificateType { get; set; }
        public string Signatures { get; set; }
        public string RawData { get; set; }

        public bool IsRevoked { get; set; }

    }
}
