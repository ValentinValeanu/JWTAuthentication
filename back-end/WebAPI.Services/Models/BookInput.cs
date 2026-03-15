using System.ComponentModel.DataAnnotations;

namespace WebAPI.Services.Models
{
    public class BookInput
    {
        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Author { get; set; }

        [Required]
        public int PageCount { get; set; }

        public int Edition { get; set; } = 1;

        public string? Language { get; set; }

        public string? Category { get; set; }

        public string? TargetGroup { get; set; }

        public DateTime LaunchDate { get; set; }
    }
}
