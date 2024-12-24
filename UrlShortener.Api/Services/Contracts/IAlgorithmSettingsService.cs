using UrlShortener.Api.Models;

namespace UrlShortener.Api.Services.Contracts
{
    public interface IAlgorithmSettingsService
    {
        Task<AlgorithmSettings> GetAlgorithmSettingsAsync();
        Task UpdateAlgorithmSettingsAsync(AlgorithmSettings algorithm);
    }
}