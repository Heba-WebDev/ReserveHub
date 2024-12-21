using Shared.DataTransferObjects.Rooms;
namespace Service.Contracts;

public interface IRoomService
{
    Task<RoomResponseDto> CreateRoom(CreateRoomRequestDto createRoomRequestDto);
}
