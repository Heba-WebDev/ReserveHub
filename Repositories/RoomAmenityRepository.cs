using Contracts.Repositories;
using Entities.Models;
namespace Repositories;

public class RoomAmenityRepository : RepositoryBase<RoomAmenity>, IRoomAmenity
{
    public RoomAmenityRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    { }
}
