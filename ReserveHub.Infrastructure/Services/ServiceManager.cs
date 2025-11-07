using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ReserveHub.Application.Services.Contracts;
using ReserveHub.Domain.Repositories;
using ReserveHub.Infrastructure.Configurations;
using ReserveHub.Infrastructure.Identity;
namespace ReserveHub.Infrastructure.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthService> _authService;
    private readonly Lazy<IHotelService> _hotelService;
    private readonly Lazy<IGoogleAuthService> _googleAuthService;
    public ServiceManager(
        IRepositoryManager repositoryManager,
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        IOptions<JwtConfiguration> jwtConfiguration,
        IEmailService emailService,
        IOptions<FrontendConfiguration> frontendConfiguration,
        IConfiguration configuration,
        ICurrentUserService currentUserService)
    {
        _authService = new Lazy<IAuthService>(
            () => new AuthService(
                repositoryManager,
                mapper,
                userManager,
                jwtConfiguration,
                emailService,
                frontendConfiguration));

        _hotelService = new Lazy<IHotelService>(
            () => new HotelService(
                repositoryManager,
                userManager,
                mapper,
                currentUserService));

        _googleAuthService = new Lazy<IGoogleAuthService>(
            () => new GoogleAuthService(
                userManager,
                repositoryManager,
                mapper,
                jwtConfiguration));
    }
    public IAuthService AuthService => _authService.Value;
    public IHotelService HotelService => _hotelService.Value;

    public IGoogleAuthService GoogleAuthService => _googleAuthService.Value;
}