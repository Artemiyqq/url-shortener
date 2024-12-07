using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Api.Models
{
    public class RegisterDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be at least 3 characters long")]
        public required string Name { get; set; }
        [Required]
        [EmailAddress]
        public required string Login { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
        public required string Password { get; set; }
    }
}