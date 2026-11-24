using DAL.ClassesDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Template.QrCode.Api.ClassesDB;

namespace Template.QrCode.Api.Repos
{
    public interface ICertificatsRepository : IRepository<Certificats>
    {
        TraceMobileApp getLastCertificats(string Id);
        Certificats getCertificats(string Id,int TotalSeconds);
    }
}
