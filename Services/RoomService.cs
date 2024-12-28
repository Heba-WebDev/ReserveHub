using AutoMapper;
using Contracts.Repositories;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects.Rooms;
using Shared.RequestFeatures;
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

    public async Task<(IEnumerable<RoomResponseDto>? rooms, MetaData metaData)> GetAllRooms(RoomParameters roomParameters, bool trackChanges)
    {
        if (roomParameters.FloorNumber.HasValue && !roomParameters.ValidFloorNumber)
            throw new FloorNotFoundException(roomParameters.FloorNumber);
        var rooms = await _repository.Room.GetAllRooms(roomParameters, trackChanges);
        var dto = _mapper.Map<IEnumerable<RoomResponseDto>>(rooms);
        return (dto, rooms.MetaData);
    }

    public async Task<RoomResponseDto> GetRoomById(Guid roomId, bool trackChanges)
    {
        var roomEntity = await _repository.Room.GetRoomById(roomId, trackChanges);
        var dto = _mapper.Map<RoomResponseDto>(roomEntity);
        return dto;
    }
}
