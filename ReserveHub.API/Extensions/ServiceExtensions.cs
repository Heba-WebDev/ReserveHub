using Microsoft.EntityFrameworkCore;
using ReserveHub.Infrastructure.Repositories;

namespace ReserveHub.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigurePostgresSqlContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(opts =>
        {
            opts.UseNpgsql(configuration.GetConnectionString("Default"));
        });
}
