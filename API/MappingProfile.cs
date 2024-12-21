using AutoMapper;
using Entities.Models;
using Shared.DataTransferObjects;
using Shared.DataTransferObjects.Rooms;
using Shared.DataTransferObjects.Users;
namespace API;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomersDto>()
        .ForMember(des => des.Id, src => src.MapFrom(x => x.Id))
        .ForMember(des => des.FirstName, src => src.MapFrom(x => x.FirstName))
        .ForMember(des => des.LastName, src => src.MapFrom(x => x.LastName))
        .ForMember(des => des.Email, src => src.MapFrom(x => x.Email))
        .ForMember(des => des.Address, src => src.MapFrom(x => x.Address));

        CreateMap<CreateCustomerRequestDto, Customer>();
        CreateMap<User, UserDto>();
        CreateMap<CreateUserRequestDto, User>();
        CreateMap<UpdateUserRequestDto, User>() //ignores the null values of the dto
            .ForAllMembers(opts => opts.Condition((src, dest, scrMember) => scrMember != null));
        CreateMap<CreateRoomRequestDto, Room>();
        CreateMap<Room, RoomResponseDto>();
    }
}
