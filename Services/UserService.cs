using AutoMapper;
using Contracts.Repositories;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects.Users;
namespace Services;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    public UserService(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserDto> CreateUser(CreateUserRequestDto user)
    {
        var userEntity = _mapper.Map<User>(user);
        userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userEntity.Password);
        _repository.User.CreateUser(userEntity);
        await _repository.SaveAsync();
        var responseDto = _mapper.Map<UserDto>(userEntity);
        return responseDto;
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
