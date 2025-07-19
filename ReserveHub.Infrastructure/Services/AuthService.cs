using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ReserveHub.Application.DTOs;
using ReserveHub.Application.DTOs.Auth;
using ReserveHub.Application.Services.Contracts;
using ReserveHub.Domain.Entities;
using ReserveHub.Domain.Repositories;
using ReserveHub.Infrastructure.Configurations;
using ReserveHub.Infrastructure.Identity;
namespace ReserveHub.Infrastructure.Services;

public class AuthService : IAuthService
{
    public readonly IRepositoryManager _repository;
    public readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOptions<JwtConfiguration> _configuration;
    private readonly JwtConfiguration _jwtConfiguration;
    private User? _user;
    public AuthService(IRepositoryManager repository, IMapper mapper, UserManager<ApplicationUser> userManager, IOptions<JwtConfiguration> configuration)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
        _jwtConfiguration = _configuration.Value;
    }

    public async Task<BaseResponseDto> Register(RegisterDto dto)
    {
        var user = _mapper.Map<ApplicationUser>(dto);
        user.UserName = dto.Email;
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "Registration failed",
                Data = result.Errors.Select(e => e.Description)
            };
        }

        await _userManager.AddToRoleAsync(user, "Customer");

        return new BaseResponseDto()
        {
            Status = true,
            Message = "User successfully registered",
        };
    }

    public Task<BaseResponseDto> Login(LoginDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponseDto> RefreshToken(TokenDto dto)
    {
        throw new NotImplementedException();
    }
}