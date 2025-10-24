using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReserveHub.Application.Services.Contracts;
using ReserveHub.Domain.Entities;
using ReserveHub.Domain.Repositories;
using ReserveHub.Infrastructure.Configurations;
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
            opt.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<RepositoryContext>()
        .AddDefaultTokenProviders();
    }

    public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));

    public static void AddGoogleConfiguration(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<GoogleConfiguration>(configuration.GetSection("Authentication:Google"));

    public static void AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<EmailConfiguration>(configuration.GetSection("EmailSettings"));

    public static void AddFrontendConfiguration(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<FrontendConfiguration>(configuration.GetSection("FRONTEND_BASE_URL"));

    public static void ConfigureEmailService(this IServiceCollection services)
    {
        services.AddScoped<IEmailService, SmtpEmailService>();
    }
        
    public static void ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = configuration
       .GetSection("JwtSettings")
       .Get<JwtConfiguration>() ?? throw new InvalidOperationException("JwtSettings section is missing");

        if (string.IsNullOrWhiteSpace(jwtConfiguration.SecretKey))
            throw new InvalidOperationException("JWT SecretKey is not configured");
        if (string.IsNullOrWhiteSpace(jwtConfiguration.ValidIssuer))
            throw new InvalidOperationException("JWT ValidIssuer is not configured");
        if (string.IsNullOrWhiteSpace(jwtConfiguration.ValidAudience))
            throw new InvalidOperationException("JWT ValidAudience is not configured");

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }
        )
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfiguration.ValidIssuer,
                ValidAudience = jwtConfiguration.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };
        })
        .AddGoogle(googleOptions =>
        {
            var googleConfig = configuration.GetSection("Authentication:Google").Get<GoogleConfiguration>();
            googleOptions.ClientId = googleConfig!.ClientId;
            googleOptions.ClientSecret = googleConfig.ClientSecret;
            googleOptions.CallbackPath = "/api/auth/google-callback";
        });
    }
}
