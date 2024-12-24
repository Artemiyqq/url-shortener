using UrlShortener.Algorithm.Services.Contracts;
using System.Text.Json;
using System.Text;
using UrlShortener.Algorithm.Models;


namespace UrlShortener.Algorithm.Services.Implementations
{
    public class AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IConfiguration _configuration = configuration;

        public async Task<string> LoginAsync(string login, string password)
        {
            HttpClient client = _httpClientFactory.CreateClient();

            AccountDto accountDto = new() 
            { 
                Login = login,
                Password = password 
            };

            string json = JsonSerializer.Serialize(accountDto);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            string apiUrl = _configuration.GetConnectionString("ApiUrl") + "/api/auth/login";
            HttpResponseMessage response = await client.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode) throw new ArgumentException("Invalid login credentials.");
            
            string result = await response.Content.ReadAsStringAsync();
            string? token = JsonDocument.Parse(result).RootElement.GetProperty("token").GetString();

            if (string.IsNullOrEmpty(token)) throw new ArgumentException("Invalid login credentials.");

            return token;
        }

        public async Task<bool> IsTokenValidAsync(string token)
        {
            HttpClient client = _httpClientFactory.CreateClient();

            var tokenDto = new { Token = token };

            string json = JsonSerializer.Serialize(tokenDto);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            string apiUrl = _configuration.GetConnectionString("ApiUrl") + "/api/auth/check-token";
            HttpResponseMessage response = await client.PostAsync(apiUrl ,content);

            return response.IsSuccessStatusCode;
        }
    }
}