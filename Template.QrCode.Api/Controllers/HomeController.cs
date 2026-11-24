using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Template.QrCode.Api.Helpers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Template.QrCode.Api.Controllers
{
    [Route("")]
    [EnableCors("AllowOrigin")]

    public class HomeController : Controller
    {
        private IConfiguration _Configuration;
        private IWebHostEnvironment _env;
        public HomeController(IWebHostEnvironment env, IConfiguration configuration)
        {
            _Configuration = configuration;
            _env = env;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("")]
        public IActionResult Index()
        {
            return Ok(new { text = "Template QR Code API is online" });
        }
      

        
    }
}
