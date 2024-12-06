using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Api.Models
{
    public class AccountDto
    {
        [Required]
        public required string Login { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}