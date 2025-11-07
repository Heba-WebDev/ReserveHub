using AutoMapper;
using ReserveHub.Application.DTOs.Auth;
using ReserveHub.Application.DTOs.Hotel;
using ReserveHub.Domain.Entities;
using ReserveHub.Infrastructure.Identity;
namespace ReserveHub.Infrastructure.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterDto, ApplicationUser>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
            .ForMember(dest => dest.RefreshTokenExpiryTime, opt => opt.Ignore())
            .ForMember(dest => dest.OwnedHotels, opt => opt.Ignore())
            .ForMember(dest => dest.Reservations, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
            .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
            .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
            .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
            .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
            .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore())
            .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
            .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
            .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore());

        CreateMap<CreateHotelDto, Hotel>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.OwnerId, opt => opt.Ignore())
            .ForMember(d => d.Rooms, opt => opt.Ignore());
    }
}
