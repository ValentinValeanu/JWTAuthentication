using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace WebAPI.Data
{
    public class SandboxContextFactory : IDesignTimeDbContextFactory<SandboxContext>
    {
        public SandboxContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../WebAPI");

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<SandboxContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new SandboxContext(optionsBuilder.Options);
        }
    }
}
