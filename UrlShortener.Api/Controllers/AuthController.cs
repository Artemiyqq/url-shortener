using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Contracts;

namespace UrlShortener.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        public async Task<IActionResult> Login(AccountDto accountDto)
        {
            if (!ModelState.IsValid) return BadRequest(new { message = "Invalid data" });

            var accountId = await _authService.Login(accountDto);
            
            if (accountId == -1) return BadRequest(new { message = "Invalid login or password" });

            return Ok(accountId);
        }
    }
}
