namespace ReserveHub.Application.Services.Contracts;

public interface IServiceManager
{
    IAuthService AuthService { get; }
    IGoogleAuthService GoogleAuthService { get;  }
}
