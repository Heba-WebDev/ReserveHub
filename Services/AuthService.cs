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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.DataTransferObjects.Auth;
using Shared.DataTransferObjects.Users;
namespace Services;

public class AuthService : IAuthService
{
    public readonly IRepositoryManager _repository;
    public readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IOptions<JwtConfiguration> _configuration;
    private readonly JwtConfiguration _jwtConfiguration;
    private User? _user;
    public AuthService(IRepositoryManager repository, IMapper mapper, UserManager<User> userManager, IOptions<JwtConfiguration> configuration)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
        _jwtConfiguration = _configuration.Value;
    }
    public async Task<BasedResponseDto> Register(RegisterDto dto)
    {
        var user = _mapper.Map<User>(dto);
        user.UserName = user.Email;
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (result.Succeeded)
            await _userManager.AddToRoleAsync(user, "Customer");
        return new BasedResponseDto()
        {
            Status = true,
            Message = "User successfully registered",
        };
    }
    public async Task<BasedResponseDto> Login(LoginDto dto, bool populateExp)
    {
        var user = await ValidateUser(dto);
        if (!user)
            return new BasedResponseDto()
            {
                Status = false,
                Message = "Invalid credentials"
            };
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var refreshToken = GenerateRefreshToken();
        _user!.RefreshToken = refreshToken;
        if (populateExp)
            _user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _userManager.UpdateAsync(_user);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        var userData = _mapper.Map<UserDto>(_user);
        // must create a customer when a user is created (but when a customer is created, a user is not created)
        return new BasedResponseDto()
        {
            Status = true,
            Message = "User successfully logged in",
            Data = new {
                accessToken,
                refreshToken,
                data = userData
            }
        };
    }

    private async Task<TokenDto> CreateToken(bool populateExp)
    {
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
    public async Task<BasedResponseDto> RefreshToken(TokenDto dto)
    {
        var principal = GetPrincipalFromExpiredToken(dto.AccessToken);
        var user = await _userManager.FindByNameAsync(principal.Identity!.Name!);
        if (user == null || user.RefreshToken != dto.RefreshToken
            || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new RefreshTokenBadRequest();
        _user = user;
        TokenDto tokens = await CreateToken(populateExp: false);
        return new BasedResponseDto()
        {
            Status = true,
            Message = "Successfully refreshed token",
            Data = new {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
            }
        };
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

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.SecretKey!)),
            ValidateLifetime = true,
            ValidIssuer = _jwtConfiguration.ValidIssuer,
            ValidAudience = _jwtConfiguration.Validaudience,
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
        throw new SecurityTokenException("Invalid token");
        return principal;
    }
    private async Task<bool> ValidateUser(LoginDto dto)
    {
        _user = await _userManager.FindByEmailAsync(dto.Email);
        var result = (_user != null && await _userManager.CheckPasswordAsync(_user,
            dto.Password));
        return result;
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
            audience: _jwtConfiguration.Validaudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expries)),
            signingCredentials: signingCredentials
        );
    }
}
