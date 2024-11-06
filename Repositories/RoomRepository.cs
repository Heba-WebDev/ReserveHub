using Contracts.Repositories;
using Entities.Models;
namespace Repositories;

public class RoomRepository : RepositoryBase<Room>, IRoom
{
    public RoomRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {}
}
