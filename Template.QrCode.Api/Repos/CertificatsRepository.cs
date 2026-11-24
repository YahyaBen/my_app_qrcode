using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.QrCode.Api.ClassesDB;

namespace Template.QrCode.Api.Repos
{
    public class CertificatsRepository : Repository<Certificats>, ICertificatsRepository
    {
        //private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public CertificatsRepository(InterieurContext context) : base(context)
        {

        }

        //public List<Personne> GetDetailsPersonneByCNIE(PersonneModel pInput)
        //{

        //    try
        //    {
        //        // var lParameters = new[]{
        //        //new SqlParameter("@CNIE",pInput.CNIE),
        //        // };

        //        // var query = "exec spGetDetailsPersonneByCNIE @CNIE = @CNIE";

        //        //var query = "exec spGetDetailsPersonneByCNIE {0} = @CNIE";

        //        //var t =  GetWithRawSql("exec [dbo].[spGetDetailsPersonneByCNIE] {0}", pInput.CNIE).ToList();
        //        var t = GetPers("exec [dbo].[spGetDetailsPersonneByCNIE] @CNIE = " + pInput.PersonneCnie).FirstOrDefault();
        //        //var t =  GetWithRawSql("exec [dbo].[SP_GetPersonneByCINE] @CNIE = " + pInput.CNIE).FirstOrDefault();

        //    }
        //    catch (Exception ex)
        //    {


        //        return null;
        //    }

        //}


        //public Certificats GetCertificats(AuthModel lInput)
        //{
        //    try
        //    {

        //        return GetWithRawSql("exec [dbo].[spGetCertificats]").OrderByDescending(x => x.DateNextSynchronization).FirstOrDefault();

        //    }
        //    catch (Exception ex)
        //    {


        //        return null;
        //    }
        //}

        public Certificats getCertificats(string Id,int TotalSeconds)
        {
            try
            {
                var queryGetCertif = "exec spGetCertificats";
                
                var result = getLastCertificats(Id);
                if(result != null)
                {

                    var diff = (DateTime.Now - result.DateInsertion.Value).TotalSeconds;

                    if (diff < TotalSeconds) return null;
                    else
                    {
                        return GetCertificats(queryGetCertif);
                    }
                }

                  return GetCertificats(queryGetCertif);
            }
            catch (Exception ex)
            {
                return null;

            }
        }

            public TraceMobileApp getLastCertificats(string Id)
            {
                try
                {

                var lParameters = new[]{
              new SqlParameter("@loginId",Id),
               };

                var query = "exec spGetLastCertificats @loginId = @loginId";

                    return GetLastCertificatsFromTrace(query, lParameters);
                }
                catch (Exception ex)
                {
                    return null;

                }
            }

    }
        

}
