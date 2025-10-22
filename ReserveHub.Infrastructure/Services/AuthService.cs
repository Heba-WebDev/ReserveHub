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
    private readonly IEmailService _emailService;
    public AuthService(IRepositoryManager repository, IMapper mapper, UserManager<ApplicationUser> userManager, IOptions<JwtConfiguration> configuration, IEmailService emailService)
    {
        _repository = repository;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
        _jwtConfiguration = _configuration.Value;
        _emailService = emailService;
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
        user.EmailConfirmed = false;

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

        await _userManager.AddToRoleAsync(user, dto.Role);
        
        // Generate email confirmation token using the default provider
        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = $"http://localhost:3000/confirm-email?email={Uri.EscapeDataString(user.Email!)}&token={Uri.EscapeDataString(confirmationToken)}";
        
        await _emailService.SendEmailConfirmationAsync(user.Email!, confirmationLink);
    

        return new BaseResponseDto()
        {
            Status = true,
            Message = "Registration successful. Please check your email to confirm your account.",
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

        if (!user.EmailConfirmed)
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "Please confirm your email before logging in. Check your inbox for a confirmation email.",
                Data = new { EmailConfirmed = false, Email = user.Email }
            };
        }

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

    public async Task<BaseResponseDto> ConfirmEmailAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        
        if (user == null)
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "User not found"
            };
        }
        
        if (user.EmailConfirmed)
        {
            return new BaseResponseDto()
            {
                Status = true,
                Message = "Email already confirmed"
            };
        }
        
        var decodedToken = Uri.UnescapeDataString(token);
        
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
        
        if (!result.Succeeded)
        {
            
            return new BaseResponseDto()
            {
                Status = false,
                Message = "Invalid or expired confirmation token",
                Data = result.Errors.Select(e => e.Description)
            };
        }
        
        return new BaseResponseDto()
        {
            Status = true,
            Message = "Email confirmed successfully. You can now log in."
        };
    }

    public async Task<BaseResponseDto> ResendConfirmationEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
    
        if (user == null)
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "User not found"
            };
        }
    
        if (user.EmailConfirmed)
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "Email already confirmed"
            };
        }
    

        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = $"http://localhost:3000/confirm-email?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(confirmationToken)}";
        
        await _emailService.SendEmailConfirmationAsync(user.Email, confirmationLink);
        
        return new BaseResponseDto()
        {
            Status = true,
            Message = "Confirmation email sent successfully"
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
        var updateResult = await _userManager.UpdateAsync(_user);
        if (!updateResult.Succeeded)
            throw new InvalidOperationException("Failed to persist refresh token");
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
        if (!int.TryParse(_jwtConfiguration.Expires, out var minutes) || minutes <= 0)
            throw new InvalidOperationException("JWT Expires is not configured or invalid");

        return new JwtSecurityToken
        (
            issuer: _jwtConfiguration.ValidIssuer,
            audience: _jwtConfiguration.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(minutes),
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

    public async Task<BaseResponseDto> SendPasswordResetEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return new BaseResponseDto()
            {
                Status = true,
                Message = "Email sent successfully.",

            };

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var confirmationLink = $"http://localhost:3000/reset-password?email={Uri.EscapeDataString(user.Email!)}&token={Uri.EscapeDataString(token)}";

        await _emailService.SendPasswordResetAsync(user.Email!, confirmationLink);

        return new BaseResponseDto()
        {
            Status = true,
            Message = "Email sent successfully.",
        };

    }
    
    public async Task<BaseResponseDto> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        
        if (user == null)
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "User not found"
            };
        }
        
        var decodedToken = Uri.UnescapeDataString(dto.Token);
        
        var isValidToken = await _userManager.VerifyUserTokenAsync(
            user, 
            _userManager.Options.Tokens.PasswordResetTokenProvider, 
            "ResetPassword", 
            decodedToken
        );

        if (!isValidToken)
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "Invalid or expired reset token"
            };
        }

        var result = await _userManager.ResetPasswordAsync(user, decodedToken, dto.Password);

        if (!result.Succeeded)
        {
            return new BaseResponseDto()
            {
                Status = false,
                Message = "An error occurred while updating the password.",
                Data = result.Errors.Select(e => e.Description)
            };
        }
        
        return new BaseResponseDto()
        {
            Status = true,
            Message = "Password successfully updated."
        };
    }
}