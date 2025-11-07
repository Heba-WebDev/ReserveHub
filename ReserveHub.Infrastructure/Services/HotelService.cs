using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ReserveHub.Application.DTOs;
using ReserveHub.Application.DTOs.Hotel;
using ReserveHub.Application.Services.Contracts;
using ReserveHub.Domain.Entities;
using ReserveHub.Domain.Repositories;
using ReserveHub.Infrastructure.Identity;
namespace ReserveHub.Infrastructure.Services;

public class HotelService : IHotelService
{
    public readonly IRepositoryManager _repository;
    public readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;
    private readonly UserManager<ApplicationUser> _userManager;

    public HotelService(IRepositoryManager repositoryManager, UserManager<ApplicationUser> userManager, IMapper mapper, ICurrentUserService currentUser)
    {
        _mapper = mapper;
        _repository = repositoryManager;
        _currentUser = currentUser;
        _userManager = userManager;
    }
    
    public async Task<BaseResponseDto> Create(CreateHotelDto dto)
    {
        Console.WriteLine($"[HotelService.Create] Extracted UserId: {_currentUser.UserId ?? "NULL"}");
        
        if (_currentUser.UserId == null)
        {
            Console.WriteLine("[HotelService.Create] UserId is null - returning authentication error");
            return new BaseResponseDto()
            {
                Status = false,
                Message = "User not authenticated"
            };
        }

        Console.WriteLine($"[HotelService.Create] Looking up user with ID: {_currentUser.UserId}");
        var user = await _userManager.FindByIdAsync(_currentUser.UserId);
        Console.WriteLine($"[HotelService.Create] User found: {(user != null ? $"Yes (Email: {user.Email})" : "No")}");
        if (user == null)
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "User not found"
            };
        }

        var isOwner = await _userManager.IsInRoleAsync(user, "Owner");
        if (!isOwner)
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "Only users with Owner role can create hotels"
            };
        }

        var mapHotel = _mapper.Map<Hotel>(dto);
        mapHotel.OwnerId = _currentUser.UserId;

        try
        {
            await _repository.Hotel.CreateHotelAsync(mapHotel);
            await _repository.SaveAsync();

            return new BaseResponseDto()
            {
                Status = true,
                Message = "Hotel created successfully",
            };
        }
        catch 
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "An error occurred while creating the hotel.",
            };
        }
    }

    public async Task<BaseResponseDto> Delete(string hotelId, string ownerID)
    {
        var hotel = await _repository.Hotel.GetHotelByIdAsync(Guid.Parse(hotelId), true);
        if (hotel == null)
            return new BaseResponseDto()
            {
                Status = false,
                Message = "Hotel not found"
            };

        if (hotel.OwnerId != ownerID)
            return new BaseResponseDto()
            {
                Status = false,
                Message = "User unauthorized to perform this action"
            };

        try
        {
            await _repository.Hotel.DeleteHotelAsync(hotel);
            await _repository.SaveAsync();
            
            return new BaseResponseDto()
            {
                Status = true,
                Message = "Hotel successfully deleted",
            };
        }
        catch
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "An error occured. Failed to delete the hotel",
            };
        }
    }

    public async Task<BaseResponseDto> GetHotelById(string hotelId)
    {
        var hotel = await _repository.Hotel.GetHotelByIdAsync(Guid.Parse(hotelId), true);
        if (hotel == null)
            return new BaseResponseDto()
            {
                Status = false,
                Message = "Hotel not found"
            };
        return new BaseResponseDto()
        {
            Status = true,
            Message = "Hotel successfully retrieved",
            Data = hotel,
        };
    }

    public async Task<BaseResponseDto> Update(string hotelId, UpdateHotelDto dto)
    {
        if (_currentUser.UserId == null)
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "User not authenticated"
            };
        }

        var hotel = await _repository.Hotel.GetHotelByIdAsync(Guid.Parse(hotelId), true);

        if (hotel == null)
            return new BaseResponseDto()
            {
                Status = false,
                Message = "Hotel not found",
            };

        if (hotel.OwnerId != _currentUser.UserId)
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "User unauthorized to perform this action"
            };
        }

        if (dto.Name != null)
            hotel.Name = dto.Name;

        if (dto.Description != null)
            hotel.Description = dto.Description;

        if (dto.Country != null)
            hotel.Country = dto.Country;

        if (dto.Location != null)
            hotel.Location = dto.Location;

        try
        {
            await _repository.Hotel.UpdateHotelAsync(hotel);
            await _repository.SaveAsync();

            return new BaseResponseDto()
            {
                Status = true,
                Message = "Hotel updated successfully",
            };
        }
        catch
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "An error occurred while updating the hotel.",
            };
        }
    }
    
}