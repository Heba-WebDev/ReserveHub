using Microsoft.EntityFrameworkCore;
using ReserveHub.Domain.Entities;
using ReserveHub.Domain.Repositories;
namespace ReserveHub.Infrastructure.Repositories;

public class HotelRepository : RepositoryBase<Hotel>, IRepositoryHotel
{
    public HotelRepository(RepositoryContext repositoryContext): base(repositoryContext) {}
    
    public async Task CreateHotelAsync(Hotel hotel) => await Create(hotel);

    public async Task DeleteHotelAsync(Hotel hotel) => await Delete(hotel);

    public async Task<IEnumerable<Hotel>> GetAllHotelsAsync(bool trackChanges) =>
        await FindAll(trackChanges).ToListAsync();

    public async Task<IEnumerable<Hotel>> GetAllHotelsAsync(int pageNumber, int pageSize, bool trackChanges)
    {
        var skip = (pageNumber - 1) * pageSize;
        return await FindAll(trackChanges)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetHotelsCountAsync(bool trackChanges) =>
        await FindAll(trackChanges).CountAsync();

    public async Task<Hotel?> GetHotelByIdAsync(Guid hotelId, bool trackChanges) =>
        await FindByCondition(x => x.Id.Equals(hotelId), trackChanges).SingleOrDefaultAsync();

    public async Task<IEnumerable<Hotel>> GetHotelsByOwnerAsync(string ownerId, bool trackChanges) =>
        await FindByCondition(x => x.OwnerId.Equals(ownerId), trackChanges).ToListAsync();

    public async Task UpdateHotelAsync(Hotel hotel) => await Update(hotel);
}