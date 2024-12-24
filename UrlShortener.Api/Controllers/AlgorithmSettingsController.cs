using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Contracts;

namespace UrlShortener.Api.Controllers
{
    [ApiController]
    [Route("api/algorithm-settings")]
    [Authorize]
    public class AlgorithmSettingsController(IAlgorithmSettingsService algorithmSettings) : ControllerBase
    {
        private readonly IAlgorithmSettingsService _algorithmSettingsService = algorithmSettings;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            try
            {
                var algorithmSettings = await _algorithmSettingsService.GetAlgorithmSettingsAsync();
                return Ok(algorithmSettings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(AlgorithmSettings algorithm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _algorithmSettingsService.UpdateAlgorithmSettingsAsync(algorithm);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}