namespace SmartQRCoffee.Services.DTOs;

/// <summary>
/// DTO để gửi Refresh Token lên server để lấy Access Token mới.
/// </summary>
public class ExchangeTokenDto
{
    public string RefreshToken { get; set; } = null!;
}

/// <summary>
/// Kết quả trả về khi Exchange Token thành công.
/// </summary>
public class ExchangeTokenResponseDto
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiry { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
}
