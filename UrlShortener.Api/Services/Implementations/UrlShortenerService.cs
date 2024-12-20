using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Data;
using UrlShortener.Api.Models;
using UrlShortener.Api.Services.Contracts;

namespace UrlShortener.Api.Services.Implementations
{
    public class UrlShortenerService(UrlShortenerDbContext context) : IUrlShortenerService
    {
        private readonly UrlShortenerDbContext _context = context;
        private const string Alphabets = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int ShortUrlLength = 6;


        public async Task<ShortenedUrlDto> ShortenUrlAsync(string longUrl, int accountId)
        {
            if (longUrl.Length == 0) throw new ArgumentException("URL cannot be empty", nameof(longUrl));

            bool longUrlExists = await _context.ShortenedUrls.AnyAsync(x => x.LongUrl == longUrl);
            if (longUrlExists) throw new ArgumentException("URL already exists");

            string shortUrl = GenerateShortUrlAsync();
            ShortenedUrl shortenedUrl = new(longUrl, shortUrl, accountId);

            await _context.ShortenedUrls.AddAsync(shortenedUrl);
            await _context.SaveChangesAsync();

            ShortenedUrl? savedShortenedUrl = await _context.ShortenedUrls.FirstAsync(x => x.ShortUrl == shortUrl);

            return new ShortenedUrlDto(savedShortenedUrl.Id, savedShortenedUrl.ShortUrl, savedShortenedUrl.LongUrl);
        }

        private static string GenerateShortUrlAsync()
        {
            Random random = new();
    
            char[] newShortUrl = new char[ShortUrlLength];

            for (int i = 0; i < ShortUrlLength; i++)
            {
                newShortUrl[i] = Alphabets[random.Next(Alphabets.Length)];
            }

            return new string(newShortUrl);
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
            };

            return shortUrlInfoDto;
        }

        public async Task<string> GetLongUrlAsync(string shortUrl)
        {
            ShortenedUrl? shortenedUrl = await _context.ShortenedUrls.FirstOrDefaultAsync(x => x.ShortUrl == shortUrl);

            if (shortenedUrl == null) throw new ArgumentException("Invalid short URL");

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

