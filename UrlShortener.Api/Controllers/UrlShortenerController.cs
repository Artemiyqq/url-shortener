using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Contracts;

namespace UrlShortener.Api.Controllers
{
    [ApiController]
    [Route("api/url-shortener")]
    [Authorize]
    public class UrlShortenerController(IUrlShortenerService urlShortenerService) : ControllerBase
    {
        private readonly IUrlShortenerService _urlShortenerService = urlShortenerService;

        [HttpGet("{urlIndex}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetByIndex(int urlIndex)
        {
            try
            {
                var shortenedUrl = await _urlShortenerService.GetUrlByIndexAsync(urlIndex);
                return Ok(shortenedUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("long-url/{shortUrl}"), AllowAnonymous]
        public async Task<IActionResult> GetByShortUrl(string shortUrl)
        {
            try
            {
                string longUrl = await _urlShortenerService.GetLongUrlAsync(shortUrl);
                LongUrlDto longUrlDto = new() { LongUrl = longUrl };
                return Ok(longUrlDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var shortenedUrls = await _urlShortenerService.GetAllUrlsAsync();
                return Ok(shortenedUrls);
            }
            catch (Exception ex)
            {
                return BadRequest( new { message = ex.Message } );
            }
        }

        [HttpPost("shorten")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<ShortenedUrlDto>> ShortenUrl(LongUrlDto longUrlDto)
        {
            string? accountId = User.FindFirst("id")?.Value;
            if (accountId == null)
            {
                return BadRequest("Unauthorized");
            }

            int parsedAccountId = int.Parse(accountId);
            try
            {
                ShortenedUrlDto shortenedUrl = await _urlShortenerService.ShortenUrlAsync(longUrlDto.LongUrl, parsedAccountId);
                return CreatedAtAction(nameof(GetByIndex), new { urlIndex = shortenedUrl.Id }, shortenedUrl);
            }
            catch (Exception ex)
            {
                return BadRequest( new { message = ex.Message });
            }
        }

        [HttpDelete("{urlIndex}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteByIndex(int urlIndex)
        {
            try
            {
                string? accountId = User.FindFirst("id")?.Value;
                if (accountId == null)
                {
                    return BadRequest("Unauthorized");
                }
                var role = User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
                bool isAdmin = role == "Admin";

                if (!isAdmin)
                {
                    ShortenedUrl url = await _urlShortenerService.GetUrlByIndexAsync(urlIndex);

                    int parsedAccountId = int.Parse(accountId);
                    if (url.CreatedBy != parsedAccountId) return Forbid();
                }

                await _urlShortenerService.DeleteUrlByIndexAsync(urlIndex);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}