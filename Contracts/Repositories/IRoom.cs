using Entities.Models;
using Shared.RequestFeatures;
namespace Contracts.Repositories;

public interface IRoom
{
    void CreateRoom(Room room);
    Task<PagedList<Room>> GetAllRooms(RoomParameters roomParameters, bool trackChanges);
}
