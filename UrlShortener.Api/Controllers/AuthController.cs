using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Contracts;

namespace UrlShortener.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService, ITokenService jwtTokenService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ITokenService _jwtTokenService = jwtTokenService;

        [HttpPost("login")]
        public async Task<IActionResult> Login(AccountDto accountDto)
        {
            if (!ModelState.IsValid) return BadRequest(new { message = "Invalid data" });

            Account account;
            string token;
            try
            {
                account = await _authService.LoginAsync(accountDto);
                token = _jwtTokenService.GenerateToken(account.Id, account.IsAdmin);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(new { message = "Invalid data" });
            try
            {
                await _authService.RegisterAsync(registerDto);
                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterDto registerDto)
        {
            if (!ModelState.IsValid) return BadRequest(new { message = "Invalid data" });

            try
            {
                await _authService.RegisterAdminAsync(registerDto);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("check-token")]
        public async Task<IActionResult> CheckToken(TokenDto tokenDto)
        {
            if (string.IsNullOrEmpty(tokenDto.Token)) return BadRequest(new { message = "Invalid token" });

            try
            {
                bool isValid = _jwtTokenService.IsValidToken(tokenDto.Token);

                if (!isValid)
                {
                    return BadRequest(new { message = "Invalid token" });
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

