namespace UrlShortener.Api.Models
{
    public class ShortenedUrlDto(int id, string shortUrl, string longUrl)
    {
        public int Id { get; set; } = id;
        public string ShortUrl { get; set; } = shortUrl;
        public string LongUrl { get; set; } = longUrl;
    }
}