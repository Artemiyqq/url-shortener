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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var shortenedUrls = await _urlShortenerService.GetAllUrlsAsync();
                return Ok(shortenedUrls);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("shorten-url/{longUrl}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> ShortenUrl(string longUrl)
        {
            string? accountId = User.FindFirst("id")?.Value;
            if (accountId == null)
            {
                return BadRequest("Unauthorized");
            }

            int parsedAccountId = int.Parse(accountId);
            try
            {
                var shortenedUrl = await _urlShortenerService.ShortenUrlAsync(longUrl, parsedAccountId);
                return CreatedAtAction(nameof(GetByIndex), new { urlIndex = shortenedUrl.Id }, shortenedUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
                bool isAdmin = User.FindFirst("role")?.Value == "Admin";

                if (!isAdmin)
                {
                    ShortenedUrl url = await _urlShortenerService.GetUrlByIndexAsync(urlIndex);

                    int parsedAccountId = int.Parse(accountId);
                    if (url.CreatedBy != parsedAccountId) return Forbid("You can only delete your own URLs");
                }

                await _urlShortenerService.DeleteUrlByIndexAsync(urlIndex);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}