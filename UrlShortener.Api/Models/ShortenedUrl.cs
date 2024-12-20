using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Api.Models
{
    public class ShortenedUrl(string longUrl, string shortUrl, int createdBy)
    {
        public int Id { get; set; }
        public string ShortUrl { get; set; } = shortUrl;
        [StringLength(1500)]
        public string LongUrl { get; set; } = longUrl;
        public int CreatedBy { get; set; } = createdBy;
        public Account Account { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int UsageCount { get; set; } = 0;
    }
}