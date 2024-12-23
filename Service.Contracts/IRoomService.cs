using Entities.Models;
using Shared.DataTransferObjects.Rooms;
using Shared.RequestFeatures;
namespace Service.Contracts;

public interface IRoomService
{
    Task<RoomResponseDto> CreateRoom(CreateRoomRequestDto createRoomRequestDto);
    Task<(IEnumerable<RoomResponseDto>? rooms, MetaData metaData)> GetAllRooms(RoomParameters roomParameters, bool trackChanges);
}
