using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ReserveHub.Application.Services.Contracts;
using ReserveHub.Domain.Entities;
using ReserveHub.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using ReserveHub.Infrastructure.Configurations;
using ReserveHub.Infrastructure.Identity;
namespace ReserveHub.Infrastructure.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthService> _authService;
    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, UserManager<ApplicationUser> userManager, IOptions<JwtConfiguration> configuration)
    {
        _authService = new Lazy<IAuthService>(() => new AuthService(repositoryManager, mapper, userManager, configuration));
    }
    public IAuthService AuthService => _authService.Value;
}