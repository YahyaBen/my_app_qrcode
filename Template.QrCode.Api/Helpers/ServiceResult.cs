using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template.QrCode.Api.Helpers
{
    public class ServiceResult<TEntity> where TEntity : class
    {

        public int Statut { get; set; }
        public string StatutLibelle { get; set; }
        public TEntity ObjectResult { get; set; }
 

    }
}
