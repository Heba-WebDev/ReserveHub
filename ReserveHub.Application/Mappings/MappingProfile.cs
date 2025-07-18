using AutoMapper;
using ReserveHub.Application.DTOs.Auth;
using ReserveHub.Domain.Entities;
namespace ReserveHub.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterDto, User>();
    }
}
