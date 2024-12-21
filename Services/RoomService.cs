using AutoMapper;
using Contracts.Repositories;
using Service.Contracts;
using Shared.DataTransferObjects.Rooms;
namespace Services;

internal sealed class RoomService : IRoomService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public RoomService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RoomResponseDto> CreateRoom(CreateRoomRequestDto createRoomRequestDto)
    {
        var roomEntity = _mapper.Map<Entities.Models.Room>(createRoomRequestDto);
        _repository.Room.CreateRoom(roomEntity);
        await _repository.SaveAsync();
        var responseDto = _mapper.Map<RoomResponseDto>(roomEntity);
        return responseDto;
    }
}
