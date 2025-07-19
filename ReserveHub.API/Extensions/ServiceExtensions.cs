using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReserveHub.Application.Services.Contracts;
using ReserveHub.Domain.Entities;
using ReserveHub.Domain.Repositories;
using ReserveHub.Infrastructure.Identity;
using ReserveHub.Infrastructure.Repositories;
using ReserveHub.Infrastructure.Services;

namespace ReserveHub.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigurePostgresSqlContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(opts =>
        {
            opts.UseNpgsql(configuration.GetConnectionString("Default"));
        });

    public static void ConfigureRepositoryManager(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureServiceManager(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<ApplicationUser, IdentityRole>(opt =>
        {
            opt.Password.RequireDigit = true;
            opt.Password.RequireLowercase = true;
            opt.Password.RequireUppercase = true;
            opt.Password.RequireNonAlphanumeric = true;
            opt.Password.RequiredLength = 8;
            opt.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<RepositoryContext>()
        .AddDefaultTokenProviders();
    }
}
