using System.Text;
using System.Text.Json;
using UrlShortener.Algorithm.Models;
using UrlShortener.Algorithm.Services.Contracts;

namespace UrlShortener.Algorithm.Services.Implementations
{
    public class AlgorithmSettingsService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : IAlgorithmSettingsService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IConfiguration _configuration = configuration;
        public JsonSerializerOptions JsonSerializerOptions { get; set; } = new() { PropertyNameCaseInsensitive = true };

        public async Task<AlgorithmSettings> GetAlgorithmSettingsAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string apiUrl = _configuration.GetConnectionString("ApiUrl") + "/api/algorithm-settings";

            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode) throw new Exception("Failed to get algorithm settings.");

            string json = await response.Content.ReadAsStringAsync();
            AlgorithmSettings algorithmSettings = JsonSerializer.Deserialize<AlgorithmSettings>(json, JsonSerializerOptions)!;

            return algorithmSettings;
        }

        public async Task UpdateAlgorithmSettingsAsync(AlgorithmSettings algorithm, string token)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            string apiUrl = _configuration.GetConnectionString("ApiUrl") + "/api/algorithm-settings";

            string json = JsonSerializer.Serialize(algorithm);
            StringContent content = new(json, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await client.PutAsync(apiUrl, content);

            if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedAccessException("Access denied.");
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to update algorithm settings.");
            }
        }
    }
}