using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Api.Models
{
    public class AccountDto
    {
        [Required]
        [EmailAddress]
        public required string Login { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        public required string Password { get; set; }
    }
}