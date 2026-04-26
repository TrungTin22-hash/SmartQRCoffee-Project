using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartQRCoffee.Repositories.Models;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using SmartQRCoffee.Services.Contracts;
using SmartQRCoffee.Services.DTOs;

namespace SmartQRCoffee.Services.Implementations;

/// <summary>
/// JWT Token Service:
/// - Login: xác thực user → tạo RefreshToken (lưu DB) + AccessToken → trả về client
/// - ExchangeToken: nhận RefreshToken → validate → tạo AccessToken mới → trả về
/// - RevokeRefreshToken: xóa RefreshToken trong DB (logout)
/// </summary>
public class JwtTokenService : IJwtTokenService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    // Access Token sống ngắn (15 phút)
    private const int AccessTokenExpiryMinutes = 15;
    // Refresh Token sống dài (7 ngày)
    private const int RefreshTokenExpiryDays = 7;

    public JwtTokenService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    /// <summary>
    /// Login: xác thực username/password, trả về AccessToken + RefreshToken.
    /// </summary>
    public async Task<AuthResponseDto> LoginAsync(UserLoginDto dto)
    {
        // 1. Tìm user theo username
        var user = await _userRepository.GetUserByUsernameAsync(dto.Username);

        if (user == null || !user.IsActive || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            throw new Exception("Tên đăng nhập hoặc mật khẩu không đúng.");
        }

        // 2. Tạo UserDto
        var userDto = new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            RoleId = user.RoleId,
            RoleName = user.Role?.RoleName ?? string.Empty
        };

        // 3. Tạo Access Token (sống ngắn)
        var accessTokenExpiry = DateTime.UtcNow.AddMinutes(AccessTokenExpiryMinutes);
        var accessToken = GenerateAccessToken(user, userDto.RoleName, accessTokenExpiry);

        // 4. Tạo Refresh Token (random string, lưu vào DB)
        var refreshToken = GenerateRefreshToken();

        // 5. Lưu Refresh Token vào DB
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(RefreshTokenExpiryDays);
        await _userRepository.UpdateUserAsync(user);

        // 6. Trả về cả hai token
        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiry = accessTokenExpiry,
            User = userDto
        };
    }

    /// <summary>
    /// Exchange Token: nhận RefreshToken → validate → trả về AccessToken mới.
    /// </summary>
    public async Task<ExchangeTokenResponseDto> ExchangeTokenAsync(ExchangeTokenDto dto)
    {
        // 1. Tìm user theo RefreshToken
        var user = await _userRepository.GetUserByRefreshTokenAsync(dto.RefreshToken);

        if (user == null)
        {
            throw new Exception("Refresh Token không hợp lệ.");
        }

        // 2. Kiểm tra Refresh Token đã hết hạn chưa
        if (user.RefreshTokenExpiryTime == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            // Xóa refresh token hết hạn
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _userRepository.UpdateUserAsync(user);

            throw new Exception("Refresh Token đã hết hạn. Vui lòng đăng nhập lại.");
        }

        // 3. Kiểm tra user còn active không
        if (!user.IsActive)
        {
            throw new Exception("Tài khoản đã bị vô hiệu hóa.");
        }

        // 4. Tạo Access Token mới
        var roleName = user.Role?.RoleName ?? string.Empty;
        var accessTokenExpiry = DateTime.UtcNow.AddMinutes(AccessTokenExpiryMinutes);
        var accessToken = GenerateAccessToken(user, roleName, accessTokenExpiry);

        // 4.5 Tạo Refresh Token mới (Rotation)
        var newRefreshToken = GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(RefreshTokenExpiryDays);
        await _userRepository.UpdateUserAsync(user);

        // 5. Trả về Access Token và Refresh Token mới
        return new ExchangeTokenResponseDto
        {
            AccessToken = accessToken,
            AccessTokenExpiry = accessTokenExpiry,
            RefreshToken = newRefreshToken
        };
    }

    /// <summary>
    /// Revoke Refresh Token: xóa refresh token trong DB (dùng khi logout).
    /// </summary>
    public async Task RevokeRefreshTokenAsync(int userId)
    {
        var user = await _userRepository.GetUserAsync(userId);
        if (user == null)
        {
            throw new Exception("User không tồn tại.");
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _userRepository.UpdateUserAsync(user);
    }

    #region Private Helper Methods

    /// <summary>
    /// Tạo JWT Access Token với claims của user.
    /// </summary>
    private string GenerateAccessToken(User user, string roleName, DateTime expires)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var keyVal = _configuration["Jwt:Key"] ?? throw new Exception("JWT Key chưa được cấu hình.");
        var key = Encoding.UTF8.GetBytes(keyVal);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, roleName)
            }),
            Expires = expires,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Tạo Refresh Token: random 64 bytes → Base64 string (an toàn, không đoán được).
    /// </summary>
    private static string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    #endregion
}
