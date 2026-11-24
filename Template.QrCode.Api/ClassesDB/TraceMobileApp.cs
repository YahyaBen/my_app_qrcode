using System;
using System.Collections.Generic;

namespace Template.QrCode.Api.ClassesDB
{
    public partial class TraceMobileApp
    {
        public int Id { get; set; }
        public DateTime? DateInsertion { get; set; }
        public int? Service { get; set; }
        public int? Resultat { get; set; }
        public string KeySign { get; set; }
        public string KeyDecrypt { get; set; }
        public DateTime? DateNextSynchronization { get; set; }
        public string Token { get; set; }
        public string LoginId { get; set; }

        public virtual CodeServices ServiceNavigation { get; set; }
    }
}
