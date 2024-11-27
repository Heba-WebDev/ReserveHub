using AutoMapper;
using Contracts.Repositories;
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

    public UserDto CreateUser(CreateUserRequestDto user)
    {
        var userEntity = _mapper.Map<User>(user);
        userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userEntity.Password);
        _repository.User.CreateUser(userEntity);
        _repository.Save();
        var responseDto = _mapper.Map<UserDto>(userEntity);
        return responseDto;
    }
}