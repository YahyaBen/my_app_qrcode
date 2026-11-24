using System;
using Template.QrCode.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRCoder;
using QRCoder.Exceptions;

namespace Template.QrCode.Api.Controllers
{
    [ApiController]
    [Route("")]
    public sealed class QrCodeController : ControllerBase
    {
        [HttpPost("GenerateQrCode")]
        [Consumes("application/json")]
        [RequestSizeLimit(16384)]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status413PayloadTooLarge)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public IActionResult GenerateQrCode([FromBody] CreateQrCodeRequest request)
        {
            if (request.Photo != null &&
                (!Uri.TryCreate(request.Photo, UriKind.Absolute, out var photoUri) ||
                 (photoUri.Scheme != Uri.UriSchemeHttp && photoUri.Scheme != Uri.UriSchemeHttps)))
            {
                ModelState.AddModelError("photo", "Photo must be an absolute HTTP or HTTPS URL.");
                return ValidationProblem(ModelState);
            }

            try
            {
                var payload = JsonConvert.SerializeObject(request);
                using var data = QRCodeGenerator.GenerateQrCode(payload, QRCodeGenerator.ECCLevel.M,
                    forceUtf8: true, eciMode: QRCodeGenerator.EciMode.Utf8);
                using var qrCode = new PngByteQRCode(data);
                return File(qrCode.GetGraphic(8), "image/png", "qrcode.png");
            }
            catch (DataTooLongException)
            {
                ModelState.AddModelError("payload", "Data exceeds QR capacity. Shorten the values or photo URL.");
                return ValidationProblem(ModelState);
            }
        }
    }
}
