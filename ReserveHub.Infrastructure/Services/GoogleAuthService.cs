using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReserveHub.Application.DTOs;
using ReserveHub.Application.DTOs.Auth;
using ReserveHub.Application.Services.Contracts;
using ReserveHub.Domain.Repositories;
using ReserveHub.Infrastructure.Configurations;
using ReserveHub.Infrastructure.Identity;
namespace ReserveHub.Infrastructure.Services;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IOptions<JwtConfiguration> _jwtConfiguration;
    private readonly JwtConfiguration _jwtConfig;
    private ApplicationUser? _user;

    public GoogleAuthService(
        UserManager<ApplicationUser> userManager,
        IRepositoryManager repository,
        IMapper mapper,
        IOptions<JwtConfiguration> jwtConfiguration)
    {
        _userManager = userManager;
        _repository = repository;
        _mapper = mapper;
        _jwtConfiguration = jwtConfiguration;
        _jwtConfig = _jwtConfiguration.Value;
    }

    public async Task<BaseResponseDto> HandleGoogleCallbackAsync(string? email, string? name, string? googleId)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(googleId))
        {
            return new BaseResponseDto
            {
                Status = false,
                Message = "Invalid Google authentication data"
            };
        }

        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser != null)
        {
            if (existingUser.GoogleId == googleId)
            {
                _user = existingUser;
                var token = await CreateToken(existingUser, populateExp: true);
                return new BaseResponseDto
                {
                    Status = true,
                    Message = "Successfully logged in with Google",
                    Data = new { token }
                };
            }
            else if (string.IsNullOrEmpty(existingUser.GoogleId))
            {
                var googleIdInUse = await _userManager.Users
                    .AnyAsync(u => u.GoogleId == googleId && u.Id != existingUser.Id);
                if (googleIdInUse)
                {
                    return new BaseResponseDto
                    {
                        Status = false,
                        Message = "This Google account is already linked to another user"
                    };
                }
                
                existingUser.GoogleId = googleId;
                var updateResult = await _userManager.UpdateAsync(existingUser);
                
                if (updateResult.Succeeded)
                {
                    var token = await CreateToken(existingUser, populateExp: true);
                    return new BaseResponseDto
                    {
                        Status = true,
                        Message = "Google account linked successfully",
                        Data = new { token }
                    };
                }
                else
                {
                    return new BaseResponseDto
                    {
                        Status = false,
                        Message = "Failed to link Google account",
                        Data = updateResult.Errors.Select(e => e.Description)
                    };
                }
            }
            else
            {
                return new BaseResponseDto
                {
                    Status = false,
                    Message = "This email is already linked to a different Google account"
                };
            }
        }
        else
        {
            var newUser = new ApplicationUser
            {
                Email = email,
                UserName = email,
                FullName = name ?? "Google User",
                GoogleId = googleId,
                EmailConfirmed = true // Google emails are pre-verified
            };

            var result = await _userManager.CreateAsync(newUser);
            
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "Customer");
                var token = await CreateToken(newUser, populateExp: true);
                
                return new BaseResponseDto
                {
                    Status = true,
                    Message = "Account created successfully with Google",
                    Data = new { token }
                };
            }
            else
            {
                return new BaseResponseDto
                {
                    Status = false,
                    Message = "Failed to create account with Google",
                    Data = result.Errors.Select(e => e.Description)
                };
            }
        }
    }

    private async Task<TokenDto> CreateToken(ApplicationUser user, bool populateExp)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (string.IsNullOrEmpty(_jwtConfig.SecretKey))
            throw new InvalidOperationException("JWT SecretKey is not configured");

        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var refreshToken = GenerateRefreshToken();
        
        user.RefreshToken = refreshToken;
        if (populateExp)
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
            throw new InvalidOperationException("Failed to persist refresh token");
            
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return new TokenDto(accessToken, refreshToken);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey!);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.Email!),
        };
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        if (!int.TryParse(_jwtConfig.Expires, out var minutes) || minutes <= 0)
            throw new InvalidOperationException("JWT Expires is not configured or invalid");

        return new JwtSecurityToken
        (
            issuer: _jwtConfig.ValidIssuer,
            audience: _jwtConfig.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(minutes),
            signingCredentials: signingCredentials
        );
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
