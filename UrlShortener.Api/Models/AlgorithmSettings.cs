using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Api.Models
{
    public class AlgorithmSettings
    {
        public int Id { get; set; }
        [Range(3, 10)]
        public int Length { get; set; }
        public bool IncludeDigits { get; set; }
        public bool IncludeLowerLetters { get; set; }
        public bool IncludeUpperLetters { get; set; }
    }
}