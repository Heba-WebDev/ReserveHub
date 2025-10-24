using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ReserveHub.Application.Services.Contracts;
using ReserveHub.Domain.Repositories;
using ReserveHub.Infrastructure.Configurations;
using ReserveHub.Infrastructure.Identity;
namespace ReserveHub.Infrastructure.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthService> _authService;
    private readonly Lazy<IGoogleAuthService> _googleAuthService;
    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, UserManager<ApplicationUser> userManager, IOptions<JwtConfiguration> configuration, IEmailService emailService, IOptions<FrontendConfiguration> frontendConfiguration)
    {
        _authService = new Lazy<IAuthService>(() => new AuthService(repositoryManager, mapper, userManager, configuration, emailService, frontendConfiguration));
        _googleAuthService = new Lazy<IGoogleAuthService>(() => new GoogleAuthService(userManager, repositoryManager, mapper, configuration));
    }
    public IAuthService AuthService => _authService.Value;

    public IGoogleAuthService GoogleAuthService => _googleAuthService.Value;
}