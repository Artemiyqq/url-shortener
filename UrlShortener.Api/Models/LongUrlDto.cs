using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Api.Models
{
    public class LongUrlDto
    {
        [Required]
        public required string LongUrl { get; set; }
    }
}