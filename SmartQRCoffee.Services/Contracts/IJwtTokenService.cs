using System.Threading.Tasks;
using SmartQRCoffee.Services.DTOs;

namespace SmartQRCoffee.Services.Contracts;

/// <summary>
/// Service xử lý JWT Token: Login trả Refresh Token, Exchange Token trả Access Token.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Login: xác thực user, trả về AuthResponseDto chứa RefreshToken + AccessToken đầu tiên.
    /// </summary>
    Task<AuthResponseDto> LoginAsync(UserLoginDto dto);

    /// <summary>
    /// Exchange Token: nhận RefreshToken, validate, rồi trả về AccessToken mới.
    /// </summary>
    Task<ExchangeTokenResponseDto> ExchangeTokenAsync(ExchangeTokenDto dto);

    /// <summary>
    /// Revoke Refresh Token (logout): xóa refresh token của user.
    /// </summary>
    Task RevokeRefreshTokenAsync(int userId);
}
