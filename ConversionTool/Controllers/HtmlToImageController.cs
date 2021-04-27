using ConversionTool.HtmlToImage;
using ConversionTool.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ConversionTool.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class HtmlToImageController : ControllerBase
    {
        private readonly ILogger<HtmlToImageController> _logger;
        private readonly IHtmlToImage _htmlToImage;

        public HtmlToImageController(ILogger<HtmlToImageController> logger, IHtmlToImage htmlToImage)
        {
            _logger = logger;
            _htmlToImage = htmlToImage;
        }

        [HttpPost("/html-to-image/png")]
        public async Task<IActionResult> Png([FromBody] HtmlToImageModel htmlToImageModel)
        {
            try
            {
                _logger.LogInformation("Converting HTML to PNG image");

                var imageAsBytes = await _htmlToImage.FromStringToPngAsync(htmlToImageModel.Html);

                return File(imageAsBytes, "image/png");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error converting HTML text to PNG image", ex);
                return BadRequest(ex);
            }
        }
    }
}
