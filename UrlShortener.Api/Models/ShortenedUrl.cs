namespace UrlShortener.Api.Models
{
    public class ShortenedUrl(string longUrl, string shortUrl, int createdBy)
    {
        public int Id { get; set; }
        public string ShortUrl { get; set; } = shortUrl;
        public string LongUrl { get; set; } = longUrl;
        public int CreatedBy { get; set; } = createdBy;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}