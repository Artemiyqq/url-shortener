using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Data;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Contracts;

namespace UrlShortener.Api.Services.Implementations
{
    public class UrlShortenerService(UrlShortenerDbContext context, IAlgorithmSettingsService algorithmSettingsService) : IUrlShortenerService
    {
        private readonly UrlShortenerDbContext _context = context;
        private readonly IAlgorithmSettingsService _algorithmSettingsService = algorithmSettingsService;

        public async Task<ShortenedUrlDto> ShortenUrlAsync(string longUrl, int accountId)
        {
            if (longUrl.Length == 0) throw new ArgumentException("URL cannot be empty", nameof(longUrl));

            bool longUrlExists = await _context.ShortenedUrls.AnyAsync(x => x.LongUrl == longUrl);
            if (longUrlExists) throw new ArgumentException("URL already exists");

            AlgorithmSettings algorithm = await _algorithmSettingsService.GetAlgorithmSettingsAsync();

            string shortUrl = GenerateShortUrlAsync(algorithm);
            ShortenedUrl shortenedUrl = new(longUrl, shortUrl, accountId);

            await _context.ShortenedUrls.AddAsync(shortenedUrl);
            await _context.SaveChangesAsync();

            ShortenedUrl? savedShortenedUrl = await _context.ShortenedUrls.FirstAsync(x => x.ShortUrl == shortUrl);

            return new ShortenedUrlDto(savedShortenedUrl.Id, savedShortenedUrl.ShortUrl, savedShortenedUrl.LongUrl);
        }

        private static string GenerateShortUrlAsync(AlgorithmSettings algorithm)
        {
            Random random = new();

            string chars = GetCharacterSet(algorithm);

            char[] newShortUrl = new char[algorithm.Length];

            for (int i = 0; i < algorithm.Length; i++)
            {
            newShortUrl[i] = chars[random.Next(chars.Length)];
            }

            return new string(newShortUrl);
        }

        private static string GetCharacterSet(AlgorithmSettings algorithm)
        {
            string chars = "";

            if (algorithm.IncludeDigits)
            {
                chars += "0123456789";
            }

            if (algorithm.IncludeLowerLetters)
            {
                chars += "abcdefghijklmnopqrstuvwxyz";
            }

            if (algorithm.IncludeUpperLetters)
            {
                chars += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            }

            if (string.IsNullOrEmpty(chars))
            {
                throw new ArgumentException("No characters available for URL generation");
            }

            return chars;
        }

        public async Task<ShortenedUrl> GetUrlByIndexAsync(int id)
        {
            ShortenedUrl? shortenedUrl = await _context.ShortenedUrls.FirstOrDefaultAsync(x => x.Id == id);

            if (shortenedUrl == null) throw new ArgumentException("Invalid id", nameof(id));

            return shortenedUrl;
        }

        public async Task<ShortUrlInfoDto> GetUrlInfoDtoAsync(int id)
        {
            ShortenedUrl? shortenedUrl = await _context.ShortenedUrls
                                                         .Include(x => x.Account)
                                                        .FirstOrDefaultAsync(x => x.Id == id);

            if (shortenedUrl is null) throw new ArgumentException("Invalid URL id");

            ShortUrlInfoDto shortUrlInfoDto = new()
            {
                CreatedBy = shortenedUrl.Account.Name,
                CreatedDate = DateOnly.FromDateTime(shortenedUrl.CreatedDate),
                UsageCount = shortenedUrl.UsageCount
            };

            return shortUrlInfoDto;
        }

        public async Task<string> GetLongUrlAsync(string shortUrl)
        {
            ShortenedUrl? shortenedUrl = await _context.ShortenedUrls.FirstOrDefaultAsync(x => x.ShortUrl == shortUrl);

            if (shortenedUrl == null) throw new ArgumentException("Invalid short URL");

            shortenedUrl.UsageCount++;
            await _context.SaveChangesAsync();

            return shortenedUrl.LongUrl;
        }

        public async Task<List<ShortenedUrlDto>> GetAllUrlsAsync()
        {
            List<ShortenedUrlDto> shortenedUrls = await _context.ShortenedUrls.Select(x =>
                new ShortenedUrlDto(x.Id, x.ShortUrl, x.LongUrl)).ToListAsync();

            return shortenedUrls;
        }

        public async Task DeleteUrlByIndexAsync(int id)
        {
            ShortenedUrl? shortenedUrl = await _context.ShortenedUrls.FirstOrDefaultAsync(x => x.Id == id);

            if (shortenedUrl == null) throw new ArgumentException("Invalid id", nameof(id));

            _context.ShortenedUrls.Remove(shortenedUrl);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task DeleteAllUrlsAsync()
        {
            await _context.ShortenedUrls.ExecuteDeleteAsync();
            return;
        }
    }
}

