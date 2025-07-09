using ReserveHub.Domain.Entities;
using ReserveHub.Domain.Repositories;
namespace ReserveHub.Infrastructure.Repositories;

public class RoomRepository : RepositoryBase<Room>, IRepositoryRoom
{
    public RoomRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    { }

    public Task CreateRoomAsync(Room room)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRoomAsync(Room room)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Room>> GetAvailableRoomsAsync(Guid hotelId, DateTime startDate, DateTime endDate, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public Task<Room?> GetRoomByIdAsync(Guid roomId, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRoomAsync(Room room)
    {
        throw new NotImplementedException();
    }
}