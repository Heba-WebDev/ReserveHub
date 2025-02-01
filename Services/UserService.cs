using AutoMapper;
using Contracts.Repositories;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Service.Contracts;
using Shared.DataTransferObjects.Users;
namespace Services;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    public UserService(IRepositoryManager repository, IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<IdentityResult> CreateUser(CreateUserRequestDto user)
    {
        var userEntity = _mapper.Map<User>(user);
        userEntity.UserName = user.Email;
        var result = await _userManager.CreateAsync(userEntity, user.Password);
        if (result.Succeeded)
            await _userManager.AddToRolesAsync(userEntity, user.Roles!);
        return result;
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
