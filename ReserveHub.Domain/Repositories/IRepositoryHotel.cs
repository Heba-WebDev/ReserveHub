using ReserveHub.Domain.Entities;
namespace ReserveHub.Domain.Repositories;

public interface IRepositoryHotel
{
    Task CreateHotelAsync(Hotel hotel);
    Task UpdateHotelAsync(Hotel hotel);
    Task DeleteHotelAsync(Hotel hotel);
    Task<Hotel?> GetHotelByIdAsync(Guid hotelId, bool trackChanges);
    Task<IEnumerable<Hotel>> GetHotelsByOwnerAsync(string ownerId, bool trackChanges);
    Task<IEnumerable<Hotel>> GetAllHotelsAsync(bool trackChanges);
    Task<IEnumerable<Hotel>> GetAllHotelsAsync(int pageNumber, int pageSize, bool trackChanges);
    Task<int> GetHotelsCountAsync(bool trackChanges);
}

