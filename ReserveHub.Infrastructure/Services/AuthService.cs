using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
    private ApplicationUser? _user;
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
        var email = await _userManager.FindByEmailAsync(dto.Email);

        if (email != null)
            return new BaseResponseDto()
            {
                Status = false,
                Message = "Registration failed",

            };

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

    public async Task<BaseResponseDto> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
            return new BaseResponseDto()
            {
                Status = false,
                Message = "Login failed. Incorrect credentials",

            };

        var pass = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!pass)
            return new BaseResponseDto()
            {
                Status = false,
                Message = "Login failed. Incorrect credentials",

            };

        _user = user;

        var token = await CreateToken(populateExp: true);

        return new BaseResponseDto()
        {
            Status = true,
            Message = "Successfully logged in",
            Data = new
            {
                token
            }
        };
    }


    private async Task<TokenDto> CreateToken(bool populateExp)
    {
        if (_user == null)
            throw new InvalidOperationException("User must be set before creating token");

        if (string.IsNullOrEmpty(_jwtConfiguration.SecretKey))
            throw new InvalidOperationException("JWT SecretKey is not configured");
    
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var refreshToken = GenerateRefreshToken();
        _user!.RefreshToken = refreshToken;
        if (populateExp)
            _user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(_user);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return new TokenDto(accessToken, refreshToken);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey!);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, _user!.Email!),
            new Claim(ClaimTypes.Name, _user!.Email!),
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
        return new JwtSecurityToken
        (
            issuer: _jwtConfiguration.ValidIssuer,
            audience: _jwtConfiguration.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
            signingCredentials: signingCredentials
        );
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public Task<BaseResponseDto> RefreshToken(TokenDto dto)
    {
        throw new NotImplementedException();
    }
}