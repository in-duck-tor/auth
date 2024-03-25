using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace InDuckTor.Auth.Infrastructure.Database
{
    public static class IdentityDbRegistration
    {
        public static IServiceCollection AddIdentityDbContext(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var configurationSection = configuration.GetSection(nameof(DatabaseSettings));
            serviceCollection.Configure<DatabaseSettings>(configurationSection);
            var databaseSettings = configurationSection.Get<DatabaseSettings>();
            ArgumentNullException.ThrowIfNull(databaseSettings, nameof(configuration));

            return serviceCollection.AddDbContext<AppIdentityDbContext>(optionsBuilder =>
            {
                optionsBuilder
                    .UseNpgsql(configuration.GetConnectionString("AuthDatabase"));
            });
        }
    }
}
