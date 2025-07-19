using AutoMapper;
using ReserveHub.Application.DTOs.Auth;
using ReserveHub.Infrastructure.Identity;
namespace ReserveHub.Infrastructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterDto, ApplicationUser>();
    }
}
