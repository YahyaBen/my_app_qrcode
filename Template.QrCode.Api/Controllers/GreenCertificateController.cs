using DCC;
using DGC;
using Template.QrCode.Api.Models;
using Template.QrCode.Api.Models.Schema;
using Template.QrCode.Api.Helpers;
using Template.QrCode.Api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using GreenCertificateDecoder = Template.QrCode.Api.Models.GreenCertificateDecoder;
using GreenCertificateEncoder = Template.QrCode.Api.Models.GreenCertificateEncoder;
using GreenCertificateVerifier = Template.QrCode.Api.Models.GreenCertificateVerifier;
using VaccinationEntry = DCC.VaccinationEntry;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Template.QrCode.Api.Controllers
{
    [Route("")]
    [EnableCors("AllowOrigin")]

    public class GreenCertificateController : Controller
    {
        private IConfiguration _Configuration;
        private IWebHostEnvironment _env;
        private readonly SignatureDbContext _context;
        private readonly ILogger<GreenCertificateController> _logger;
        static Dictionary<int, string> ListCertificate;
        private readonly ICertificateService _certificateService;
        //private readonly ICertificatsRepository _CertificatsRepository;
        //private readonly ITraceMobileAppRepository _TraceMobileAppRepository;
        public GreenCertificateController(IWebHostEnvironment env, IConfiguration configuration, SignatureDbContext context, ICertificateService certificateService, ILogger<GreenCertificateController> logger)
        {
            _Configuration = configuration;
            _env = env;
            _context = context;
            _certificateService = certificateService;
            _logger = logger;
        }
        [HttpPost("GenerateHashedInfo")]
        public IActionResult GenerateHashedInfo([FromBody] Personne Pass)
        {
            // CheckFanId Is Valid
            bool FromBackoffice = bool.TryParse(_Configuration["FromBackoffice"], out bool result) && result;
            if (!FromBackoffice)
            {
                if (!string.IsNullOrEmpty(Pass.FanId))
                {
                    int CountDemandeFanId = _context.FanIdRequests.AsNoTracking().Where(s => s.FanId == Pass.FanId.ToUpper() && s.StatusId == (int)RequestStatusCode.Validated).Count();

                    if (CountDemandeFanId == 0)
                        return BadRequest("FanId invalide");
                }
                else return BadRequest("FanId invalide");
            }
            DgCertificateImage certificate = new DgCertificateImage
            {
                FanId = Pass.FanId,
                Photo = Pass.Photo,
            };
            try
            {
                // Get cached certificate from service
                X509Certificate2 cert2 = _certificateService.GetCertificate();

                // Create CWT
                CWTNew cwt = new CWTNew();
                cwt.DGCv1 = certificate;
                cwt.ExpirationTime = DateTime.Now.AddYears(1);
                cwt.IssueAt = DateTime.Now;
                cwt.Issuer = "MA";

                string encodedDigitalCovidCert = new GreenCertificateEncoder(cert2).Encode(cwt);
                return Ok(encodedDigitalCovidCert);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GenerateHashedInfo Exception");
                return BadRequest("Une Erreur s'est produite");
            }
        }




        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("VerifyHashedInfo")]
        public async Task<IActionResult> VerifyHashedInfoNew(string Value)
        {

            try
            {
                if (!string.IsNullOrEmpty(Value))
                {

                    string Pass = Value;
                    GreenCertificateDecoder decoder = new GreenCertificateDecoder();

                    var cwt = decoder.Decode(Pass);
                    string Identifier = "";

                    var secretariat = new SecretariatService();

                    string CryptCertificatePublic = _Configuration["CryptCertificatePublic"];
                    byte[] publicKey = Convert.FromBase64String("");
                    X509Certificate2 x = X509CertificateLoader.LoadCertificateFromFile(_env.ContentRootPath + "/Certificats/" + CryptCertificatePublic);
                    var encodedDigitalCovidCert = new GreenCertificateEncoder(x);
                    string KID = encodedDigitalCovidCert.getKid();

                    List<X509Certificate2> ListCertificate = new List<X509Certificate2>();
                    X509Certificate2Collection collection = new X509Certificate2Collection();

                    ListCertificate.Add(x);

                    secretariat.AddPublicKey(KID, ListCertificate);
                    GreenCertificateVerifier verifier = new GreenCertificateVerifier(secretariat);
                    var (isvalid, reason) = await verifier.Verify(cwt);
                    CWTNew CC = cwt;


                    Personne P = new Personne
                    {
                        //Nom = cwt.DGCv1.Nom,
                        //Prenom = cwt.DGCv1.Prenom,
                        FanId = cwt.DGCv1.FanId,
                        Photo = cwt.DGCv1.Photo,
                        //identity = cwt.DGCv1.Identifiant,
                        //Nationalité = cwt.DGCv1.Nationality,
                        //IsMajeur = cwt.DGCv1.IsMajeur,

                    };
                    //if (!string.IsNullOrEmpty(cwt.DGCv1.DateOfBirthString) && DateTime.TryParse(cwt.DGCv1.DateOfBirthString, out DateTime DateNaissance))
                    //    P.DateNaissance = DateNaissance;

                    return Ok(new ServiceResult<Personne>()
                    {
                        Statut = Convert.ToInt32(isvalid),
                        StatutLibelle = isvalid ? "Valid" : "Not Valid",
                        ObjectResult = P,
                    });

                }
                else
                {

                    StatutCertificateEnum Statut = StatutCertificateEnum.Error;
                    return BadRequest(new ServiceResult<Personne>()
                    {
                        Statut = Convert.ToInt32(Statut),
                        StatutLibelle = Statut.ToString(),
                        ObjectResult = null,
                    });
                }



            }
            catch (Exception ex)
            {
                return BadRequest();

            }


        }

    }
}
