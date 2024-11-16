using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public UrlController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateShortUrl([FromBody] UrlCreateRequest request)
        {
            if (string.IsNullOrEmpty(request.OriginalUrl))
            {
                return BadRequest("Original URL is required.");
            }

            var shortUrl = await _urlService.ShortenUrlAsync(request.OriginalUrl);
            return Ok(new UrlCreateResponse { ShortUrl = $"{Request.Scheme}://{Request.Host}/api/url/{shortUrl}" });
        }

        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> GetOriginalUrl(string shortUrl)
        {
            var originalUrl = await _urlService.GetOriginalUrlAsync(shortUrl);
            if (originalUrl == null)
            {
                return NotFound();
            }

            return Redirect(originalUrl);
        }
    }
}