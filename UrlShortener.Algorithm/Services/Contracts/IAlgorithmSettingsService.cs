using UrlShortener.Algorithm.Models;

namespace UrlShortener.Algorithm.Services.Contracts
{
    public interface IAlgorithmSettingsService
    {
        Task<AlgorithmSettings> GetAlgorithmSettingsAsync();
        Task UpdateAlgorithmSettingsAsync(AlgorithmSettings algorithm, string token);   
    }
}