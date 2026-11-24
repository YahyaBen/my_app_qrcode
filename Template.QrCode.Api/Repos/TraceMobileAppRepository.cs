

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Template.QrCode.Api.ClassesDB;

namespace Template.QrCode.Api.Repos
{

    public class TraceMobileAppRepository : Repository<TraceMobileApp>, ITraceMobileAppRepository
    {

        public TraceMobileAppRepository(InterieurContext context) : base(context)
        {

        }
         
        //public async Task InsertTraceAuthentificationAsync(AuthModel pInput)
        //{
        //    TraceMobileApp lTrace = new TraceMobileApp()
        //    {
        //        DateInsertion = DateTime.Now,
        //        Service = (int)EnumType.Authentification,
        //        Token = pInput.token,
        //        Resultat = pInput.Resultat,
        //        //LoginId = pInput.LoginId
        //    };
        //    await AddAsync(lTrace);
        //}

        //public void InsertTraceAuth_PROC(AuthModel pInput)
        //{
        //    try
        //    {
        //        var lParameters = new[]{
        //      new SqlParameter("@Service",pInput.service),
        //      new SqlParameter("@Token",pInput.token),
        //      new SqlParameter("@Resultat",pInput.Resultat),
        //      new SqlParameter("@loginId",pInput.Id),
        //       };

        //        var query = "exec spInsertTraceMobileApp @Service = @Service, @Token = @Token ,@Resultat = @Resultat, @loginId = @loginId";

             
        //        insertTraceAuth_Procedure(query, lParameters);
        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //}
            //public TraceMobileApp canGenerateToken(string id)
            //{
            //    try
            //    {
            //        var lParameters = new[]{
            //  new SqlParameter("@loginId",id),
            //   };

            //        var query = "exec spGetLastToken @loginId = @loginId";
            //        TraceMobileApp res = getLastToken(query, lParameters);
            //        if (res != null)
            //        {
                    
            //        var diff = (DateTime.Now - res.DateInsertion.Value).TotalMinutes;

            //            if (diff < 5) return false;
            //            else return true;
            //        }
            //        return true;
            //    }
            //    catch (Exception ex)
            //    {
            //        return false;

            //    }
            //}

        public TraceMobileApp canGenerateTokenForShortTime(string id, int TotalSeconds)
        {
            try
            {
                var lParameters = new[]{
              new SqlParameter("@loginId",id),
               };

                var query = "exec spGetLastTokenShortTime @loginId = @loginId";
                TraceMobileApp res = getLastToken(query, lParameters);
                if (res != null)
                {

                    var diff = (DateTime.Now - res.DateInsertion.Value).TotalSeconds;

                    if (diff > 0 &&  diff < TotalSeconds) return null;
                    else
                        return res;
                }
                return res;
            }
            catch (Exception ex)
            {
                return null;

            }
        }




    }
}
