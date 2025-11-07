using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ReserveHub.Application.Services.Contracts;
namespace ReserveHub.Infrastructure.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string? UserId
    {
        get
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                Console.WriteLine("[CurrentUserService] HttpContext is null");
                return null;
            }

            var user = httpContext.User;
            if (user == null)
            {
                Console.WriteLine("[CurrentUserService] User is null");
                return null;
            }

            // Log all available claims
            Console.WriteLine("[CurrentUserService] Available claims:");
            foreach (var claim in user.Claims)
            {
                Console.WriteLine($"  - Type: {claim.Type}, Value: {claim.Value}");
            }

            // Try to find user ID
            var subClaim = user.FindFirst("sub");
            var nameIdentifierClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            
            Console.WriteLine($"[CurrentUserService] 'sub' claim: {(subClaim?.Value ?? "NOT FOUND")}");
            Console.WriteLine($"[CurrentUserService] NameIdentifier claim: {(nameIdentifierClaim?.Value ?? "NOT FOUND")}");

            var userId = subClaim?.Value ?? nameIdentifierClaim?.Value;
            Console.WriteLine($"[CurrentUserService] Final UserId: {(userId ?? "NULL")}");
            
            return userId;
        }
    }
}
