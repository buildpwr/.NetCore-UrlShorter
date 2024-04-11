using BuildPWR.UrlShorter.Data;
using BuildPWR.UrlShorter.Entities;
using BuildPWR.UrlShorter.Models;
using Microsoft.AspNetCore.Mvc;

namespace BuildPWR.UrlShorter.Controllers
{
    [Route("api/url")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly UrlDbContext _context;

        public UrlController(UrlDbContext context)
        {
            _context = context;
        }

        [HttpPost("shorten")]
        public IActionResult ShortenUrl([FromBody] UrlItemDto urlItemDto)
        {
            if (urlItemDto == null) {
                throw new ArgumentNullException(nameof(urlItemDto));
            }

            var checkItem = IsExistOriginalUrl(urlItemDto.OriginalUrl);

            if (checkItem != null) {
                return BadRequest("Record is exists... You can update method");
            }

            string shortUrl = UrlLibrary.GenerateShortUrl();

            while (IsExistShortUrl(shortUrl)) {
                shortUrl = UrlLibrary.GenerateShortUrl();
            }

            var urlItem = new UrlItem 
            { 
                BaseUrl = urlItemDto.BaseUrl,
                OriginalUrl = urlItemDto.OriginalUrl, 
                ShortUrl = UrlLibrary.GenerateShortUrl() 
            };

            _context.UrlItems.Add(urlItem);
            _context.SaveChanges();
            return Ok(urlItem);
        }

        [HttpPut("update")]
        public IActionResult UpdateUrl([FromBody] string originalUrl)
        {
            var urlItem = IsExistOriginalUrl(originalUrl);
            
            if (urlItem == null) {
                return NotFound();
            }

            urlItem.ShortUrl = UrlLibrary.GenerateShortUrl();
            _context.SaveChanges();

            return Ok(urlItem);
        }

        [HttpGet("expand/{shortUrl}")]
        public IActionResult ExpandUrl(string shortUrl)
        {
            var urlItem = _context.UrlItems.FirstOrDefault(u => u.ShortUrl == shortUrl);
            if (urlItem != null)
                return Ok(urlItem);

            return NotFound();
        }

        private bool IsExistShortUrl(string shortUrl)
        {
            return _context.UrlItems.Any(x => x.ShortUrl == shortUrl);
        }
        private UrlItem? IsExistOriginalUrl(string originalUrl)
        {
            return _context.UrlItems.FirstOrDefault(u => u.OriginalUrl == originalUrl);
        }
    }
}
