namespace UrlShortener.Api.Models
{
    public class ShortUrlInfoDto
    {
        public required string CreatedBy { get; set; }
        public required DateOnly CreatedDate { get; set; }
    }
}