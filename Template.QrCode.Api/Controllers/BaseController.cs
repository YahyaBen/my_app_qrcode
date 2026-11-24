using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Template.QrCode.Api.Controllers
{
    public class BaseController : Controller
    {
        //private readonly IApiResult _apiResult;
        public BaseController(/*IApiResult apiResult*/)
        {
            //_apiResult = apiResult;
        }
        public ObjectResult InternalServerError(Exception exception = null)
        {
            if(exception == null)
            {
               return StatusCode((int)HttpStatusCode.InternalServerError,"Unhandled server error");
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, "Internal Server Error");
        }
    }
}
