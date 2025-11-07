using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using ReserveHub.API.Controllers;
using ReserveHub.Application.Services.Contracts;
using ReserveHub.Domain.Entities;
using ReserveHub.Domain.Repositories;
using ReserveHub.Infrastructure.Configurations;
using ReserveHub.Infrastructure.Identity;
using ReserveHub.Infrastructure.Mappings;
using ReserveHub.Infrastructure.Repositories;
using ReserveHub.Infrastructure.Services;

namespace ReserveHub.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureControllers(this IServiceCollection services)
    {
        // Use a type from the API assembly to get the correct assembly reference
        services.AddControllers()
            .AddApplicationPart(typeof(AuthController).Assembly)
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use PascalCase
                options.JsonSerializerOptions.WriteIndented = true;
            });
    }

    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        // Configure authorization to return 401 for API endpoints instead of redirecting
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = null; // Don't require auth by default
            // Configure to not redirect for API endpoints
            options.AddPolicy("ApiPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
            });
        });
    }

    public static void ConfigureApplicationConfiguration(this IConfigurationBuilder configuration)
    {
        configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    }

    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);
    }

    public static void ConfigureApplicationCookieForApi(this IServiceCollection services)
    {
        // Configure Identity to not redirect for API endpoints (must be called after ConfigureIdentity)
        services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    Console.WriteLine($"[Cookie] Redirect to login intercepted for API endpoint: {context.Request.Path}");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new { message = "Unauthorized - Please use JWT Bearer token for API authentication" }));
                }
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };
            options.Events.OnRedirectToAccessDenied = context =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    Console.WriteLine($"[Cookie] Redirect to access denied intercepted for API endpoint: {context.Request.Path}");
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";
                    return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(new { message = "Forbidden" }));
                }
                context.Response.Redirect(context.RedirectUri);
                return Task.CompletedTask;
            };
        });
    }

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

    public static void ConfigureCurrentUserService(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
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
                ClockSkew = TimeSpan.Zero,
                // Map role claims correctly - JWT uses "role" but ASP.NET Core Identity uses ClaimTypes.Role
                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.Name
            };
            
            // Configure to return 401 instead of redirecting for API requests
            options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    Console.WriteLine($"[JWT] OnChallenge triggered - Error: {context.Error}, ErrorDescription: {context.ErrorDescription}");
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Unauthorized" });
                    return context.Response.WriteAsync(result);
                },
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"[JWT] Authentication failed: {context.Exception.Message}");
                    Console.WriteLine($"[JWT] Exception type: {context.Exception.GetType().Name}");
                    if (context.Exception.InnerException != null)
                    {
                        Console.WriteLine($"[JWT] Inner exception: {context.Exception.InnerException.Message}");
                    }
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine($"[JWT] Token validated successfully for user: {context.Principal?.Identity?.Name}");
                    
                    // Log all claims from the token
                    Console.WriteLine("[JWT] Claims in token:");
                    foreach (var claim in context.Principal?.Claims ?? Enumerable.Empty<Claim>())
                    {
                        Console.WriteLine($"  - Type: {claim.Type}, Value: {claim.Value}");
                    }
                    
                    // Specifically check for role claims
                    var roleClaims = (context.Principal?.Claims
                        .Where(c => c.Type == ClaimTypes.Role || c.Type == "role" || (c.Type != null && c.Type.EndsWith("/role")))
                        .ToList()) ?? new List<Claim>();
                    
                    Console.WriteLine($"[JWT] Role claims in token: {roleClaims.Count}");
                    foreach (var roleClaim in roleClaims)
                    {
                        Console.WriteLine($"  - Role: {roleClaim.Value}");
                    }
                    
                    // Check if Owner role is present
                    var hasOwnerRole = context.Principal?.IsInRole("Owner") ?? false;
                    Console.WriteLine($"[JWT] Has Owner role: {hasOwnerRole}");
                    
                    return Task.CompletedTask;
                },
                OnMessageReceived = context =>
                {
                    var token = context.Token;
                    if (!string.IsNullOrEmpty(token))
                    {
                        Console.WriteLine($"[JWT] Token received (length: {token.Length})");
                    }
                    else
                    {
                        Console.WriteLine("[JWT] No token received in request");
                    }
                    return Task.CompletedTask;
                }
            };
        })
        .AddGoogle(googleOptions =>
        {
            var googleConfig = configuration.GetSection("Authentication:Google").Get<GoogleConfiguration>();
            if (googleConfig is null ||
               string.IsNullOrWhiteSpace(googleConfig.ClientId) ||
               string.IsNullOrWhiteSpace(googleConfig.ClientSecret))
            {
                throw new InvalidOperationException("Authentication:Google:ClientId/ClientSecret are not configured.");
            }
        
            googleOptions.ClientId = googleConfig!.ClientId;
            googleOptions.ClientSecret = googleConfig.ClientSecret;
            googleOptions.CallbackPath = "/api/auth/google-callback";
        });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "ReserveHub API",
                Version = "v1",
                Description = "Hotel Reservation Management API"
            });

            c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            // Note: We don't add a global security requirement here.
            // Only endpoints with [Authorize] attribute will require authentication.
        });
    }
}
