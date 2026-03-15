namespace WebAPI.Data.Entities
{
    public class Book
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public required string Author { get; set; }

        public required int PageCount { get; set; }

        public int Edition { get; set; } = 1;

        public string? Language { get; set; }

        public string? Category { get; set; }

        public string? TargetGroup { get; set; }

        public DateTime LaunchDate { get; set; }
    }
}
