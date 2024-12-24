using UrlShortener.Api.Models;

namespace UrlShortener.Api.Services.Contracts
{
    public interface IUrlShortenerService
    {
        Task<ShortenedUrlDto> ShortenUrlAsync(string url, int accountId);
        Task<ShortenedUrl> GetUrlByIndexAsync(int index);
        Task<ShortUrlInfoDto> GetUrlInfoDtoAsync(int index);
        Task<string> GetLongUrlAsync(string shortUrl);
        Task<List<ShortenedUrlDto>> GetAllUrlsAsync();
        Task DeleteUrlByIndexAsync(int index);
    }
}