using Contracts.Repositories;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;
namespace Repositories;

public class RoomRepository : RepositoryBase<Room>, IRoom
{
    public RoomRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {}

    public void CreateRoom(Room room) => Create(room);

    public async Task<PagedList<Room>> GetAllRooms(RoomParameters roomParameters, bool trackChanges)
    {
        var query = FindAll(trackChanges);
        var totalRoom = await query.CountAsync();
        if (roomParameters.FloorNumber.HasValue && roomParameters.FloorNumber > 0 && roomParameters.FloorNumber < 5)
        {
            query = query.Where(x => x.Floor == roomParameters.FloorNumber);
            totalRoom = await query.Where(x => x.Floor == roomParameters.FloorNumber).CountAsync();
        }
        var rooms = await query
            .OrderBy(x => x.Number)
            .Skip((roomParameters.PageNumber - 1) * roomParameters.PageSize)
            .Take(roomParameters.PageSize)
            .ToListAsync();
        return PagedList<Room>.ToPagedList(rooms, totalRoom, roomParameters.PageNumber, roomParameters.PageSize);
    }

    public async Task<Room?> GetRoomById(Guid roomId, bool trackChanges)
    {
        return await FindByCondition(x => x.Id.Equals(roomId), trackChanges).SingleOrDefaultAsync();
    }
}
