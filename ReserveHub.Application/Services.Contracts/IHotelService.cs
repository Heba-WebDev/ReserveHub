using ReserveHub.Application.DTOs;
using ReserveHub.Application.DTOs.Hotel;
namespace ReserveHub.Application.Services.Contracts;

public interface IHotelService
{
    Task<BaseResponseDto> Create(CreateHotelDto dto);
    Task<BaseResponseDto> Update(string hotelId, UpdateHotelDto dto);
    Task<BaseResponseDto> Delete(string hotelId, string ownerID);
    Task<BaseResponseDto> GetHotelById(string hotelId);
}
