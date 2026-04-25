namespace WebAPI.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        public required string Email { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime BirthDate { get; set; }

        public required string Password { get; set; }

        public List<Role> Roles { get; set; } = [];
    }
}
