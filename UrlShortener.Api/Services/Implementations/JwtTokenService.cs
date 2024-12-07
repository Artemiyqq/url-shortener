using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using UrlShortener.Api.Services.Contracts;

namespace UrlShortener.Api.Services.Implementations
{
    public class JwtTokenService(IConfiguration configuration, ILogger<JwtTokenService> logger) : ITokenService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<JwtTokenService> _logger = logger;

        public string GenerateToken(int accountId, bool isAdmin)
        {
            var claims = new List<Claim>
            {
                new("id", accountId.ToString()),
                new("role", isAdmin ? "Admin" : "User")
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(14),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool IsValidToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Key"]!);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidAudience = _configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.FromSeconds(5)
                }, out _);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Invalid token");
                return false;
            }
        }
    }
}
