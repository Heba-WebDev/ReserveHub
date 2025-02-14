using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Contracts.Repositories;
using Entities.ConfigurationModels;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Shared.DataTransferObjects.Auth;
using Shared.DataTransferObjects.Users;
using Microsoft.Extensions.Options;
namespace Services;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IOptions<JwtConfiguration> _configuration;
    private readonly JwtConfiguration _jwtConfiguration;
    private User? _user;
    public UserService(IRepositoryManager repository, IMapper mapper, UserManager<User> userManager, IOptions<JwtConfiguration> configuration)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
        _jwtConfiguration = _configuration.Value;
    }

    public async Task<UserDto> GetUserById(Guid userId, bool trackChanges)
    {
        var user = await GetUserAndCheckIfItExists(userId, trackChanges);
        var response_dto = _mapper.Map<UserDto>(user);
        return response_dto;
    }

    public async Task UpdateUser(Guid userId, UpdateUserRequestDto user, bool trackChanges)
    {
        var entity = await GetUserAndCheckIfItExists(userId, trackChanges);
        _mapper.Map(user, entity);
        await _repository.SaveAsync();
    }

    public async Task<bool> ValidateUser(LoginDto dto)
    {
        _user = await _userManager.FindByEmailAsync(dto.Email);
        var result = (_user != null && await _userManager.CheckPasswordAsync(_user,
            dto.Password));
        return result;
    }

    private async Task<User?> GetUserAndCheckIfItExists(Guid userId, bool trackChanges)
    {
        var user = await _repository.User.GetUser(userId, trackChanges);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }
        return user;
    }
}
