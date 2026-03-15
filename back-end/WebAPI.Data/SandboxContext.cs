using Microsoft.EntityFrameworkCore;
using WebAPI.Data.Entities;

namespace WebAPI.Data
{
    public class SandboxContext(DbContextOptions<SandboxContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }
    }
}
