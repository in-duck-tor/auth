using System.Reflection;
using InDuckTor.Auth.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace InDuckTor.Auth.Infrastructure.Database
{
    public class AuthDbContext : DbContext
    {

        public string? Schema { get; }

        public AuthDbContext(DbContextOptions<AuthDbContext> dbContextOptions) : base(dbContextOptions)
        {
            Schema = dbContextOptions.Extensions.OfType<AuthDbContextOptionsExtension>()
                .FirstOrDefault()
                ?.Schema;
        }

        public virtual DbSet<Credentials> Credentials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
        }
    }
}
