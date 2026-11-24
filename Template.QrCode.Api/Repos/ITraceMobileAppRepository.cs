
using DAL.ClassesDB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Template.QrCode.Api.ClassesDB;

namespace Template.QrCode.Api.Repos
{

    public interface ITraceMobileAppRepository : IRepository<TraceMobileApp>
    {
        //void InsertTraceAuth_PROC(AuthModel pInput);
        //bool canGenerateToken(string id);
        TraceMobileApp canGenerateTokenForShortTime(string id,int TotalSeconds);
    }
}
