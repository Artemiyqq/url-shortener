using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UrlShortener.Api.Data;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Contracts;

namespace UrlShortener.Api.Services.Implementations
{
    public class AlgorithmSettingsService(UrlShortenerDbContext context) : IAlgorithmSettingsService
    {
        private readonly UrlShortenerDbContext _context = context;
        public async Task<AlgorithmSettings> GetAlgorithmSettingsAsync()
        {
            AlgorithmSettings algorithm = await _context.AlgorithmSettings.FirstAsync(x => x.Id == 1);
            return algorithm;
        }

        public async Task UpdateAlgorithmSettingsAsync(AlgorithmSettings algorithm)
        {
            if (!IsNewAlgorithmValid(algorithm))
            {
                throw new ArgumentException("Invalid algorithm settings");
            }
            AlgorithmSettings oldAlgorithm = await _context.AlgorithmSettings.FirstAsync(x => x.Id == 1);

            oldAlgorithm.Length = algorithm.Length;
            oldAlgorithm.IncludeUpperLetters = algorithm.IncludeUpperLetters;
            oldAlgorithm.IncludeLowerLetters = algorithm.IncludeLowerLetters;
            oldAlgorithm.IncludeDigits = algorithm.IncludeDigits;
    
            await _context.SaveChangesAsync();

            return;
        }

        public bool IsNewAlgorithmValid(AlgorithmSettings algorithm)
        {
            if (!algorithm.IncludeDigits && !algorithm.IncludeLowerLetters && !algorithm.IncludeUpperLetters)
            {
                return false;
            }
            return true;
        }
    }
}