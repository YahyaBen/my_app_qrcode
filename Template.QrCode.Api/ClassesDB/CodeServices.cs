using System;
using System.Collections.Generic;

namespace Template.QrCode.Api.ClassesDB
{
    public partial class CodeServices
    {
        public CodeServices()
        {
            TraceMobileApp = new HashSet<TraceMobileApp>();
        }

        public int CodeService { get; set; }
        public string Libelle { get; set; }

        public virtual ICollection<TraceMobileApp> TraceMobileApp { get; set; }
    }
}
