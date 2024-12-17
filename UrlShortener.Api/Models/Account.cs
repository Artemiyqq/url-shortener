namespace UrlShortener.Api.Models
{
    public class Account
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Login { get; set; }
        public required string Password { get; set; }
        public bool IsAdmin { get; set; }

        public ICollection<ShortenedUrl> ShortenedUrls { get; set;} = [];
    }
}