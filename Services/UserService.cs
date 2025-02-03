using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Contracts.Repositories;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Shared.DataTransferObjects.Auth;
using Shared.DataTransferObjects.Users;
namespace Services;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private User? _user;
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

    public async Task<bool> ValidateUser(LoginDto dto)
    {
        _user = await _userManager.FindByEmailAsync(dto.Email);
        var result = (_user != null && await _userManager.CheckPasswordAsync(_user,
            dto.Password));
        return result;
    }

    public async Task<string> CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }
    private SigningCredentials GetSigningCredentials()
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, _user!.Email!)
        };
        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var tokenOptions = new JwtSecurityToken
        (
        issuer: jwtSettings["validIssuer"],
        audience: jwtSettings["validAudience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
        signingCredentials: signingCredentials
        );
        return tokenOptions;
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
