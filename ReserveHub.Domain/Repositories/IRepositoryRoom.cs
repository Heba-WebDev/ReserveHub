using ReserveHub.Domain.Entities;
namespace ReserveHub.Domain.Repositories;

public interface IRepositoryRoom
{
    Task<Room?> GetRoomByIdAsync(Guid roomId, bool trackChanges);
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(Guid hotelId, DateTime startDate, DateTime endDate, bool trackChanges);
    Task CreateRoomAsync(Room room);
    Task UpdateRoomAsync(Room room);
    Task DeleteRoomAsync(Room room);
}
