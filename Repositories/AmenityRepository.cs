using Contracts.Repositories;
using Entities.Models;
namespace Repositories;

public class AmenityRepository : RepositoryBase<Amenity>, IAmenity
{
    public AmenityRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    { }
}
