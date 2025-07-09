using ReserveHub.Domain.Entities;
using ReserveHub.Domain.Repositories;
namespace ReserveHub.Infrastructure.Repositories;

public class HotelRepository : RepositoryBase<Hotel>, IRepositoryHotel
{
    public HotelRepository(RepositoryContext repositoryContext): base(repositoryContext) {}
    public Task CreateHotelAsync(Hotel hotel)
    {
        throw new NotImplementedException();
    }

    public Task DeleteHotelAsync(Hotel hotel)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Hotel>> GetAllHotelsAsync(bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public Task<Hotel?> GetHotelByIdAsync(Guid hotelId, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Hotel>> GetHotelsByOwnerAsync(string ownerId, bool trackChanges)
    {
        throw new NotImplementedException();
    }

    public Task UpdateHotelAsync(Hotel hotel)
    {
        throw new NotImplementedException();
    }
}