using System;
using System.Collections.Generic;

namespace Template.QrCode.Api.ClassesDB
{
    public partial class Certificats
    {
        public int Id { get; set; }
        public DateTime? DateNextSynchronization { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public int Status { get; set; }
        public DateTime? Dateinsertion { get; set; }
        public string version { get; set; }
        
    }
}
