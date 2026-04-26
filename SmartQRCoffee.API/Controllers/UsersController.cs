using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartQRCoffee.Services.Contracts;
using SmartQRCoffee.Services.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartQRCoffee.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;

    public UsersController(IUserService userService, IJwtTokenService jwtTokenService)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
    }

    /// <summary>
    /// Login: xác thực user → set AccessToken + RefreshToken vào HttpOnly Cookie.
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        try
        {
            var result = await _jwtTokenService.LoginAsync(dto);

            SetTokenCookie("access_token", result.AccessToken, result.AccessTokenExpiry);
            SetTokenCookie("refresh_token", result.RefreshToken, System.DateTime.UtcNow.AddDays(7));

            return Ok(new { Message = "Đăng nhập thành công", User = result.User });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Exchange Token: đọc RefreshToken từ Cookie → nhận AccessToken và RefreshToken mới vào Cookie.
    /// </summary>
    [HttpPost("exchange-token")]
    public async Task<IActionResult> ExchangeToken()
    {
        try
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(new { Message = "Không tìm thấy Refresh Token trong Cookie." });

            var dto = new ExchangeTokenDto { RefreshToken = refreshToken };
            var result = await _jwtTokenService.ExchangeTokenAsync(dto);

            SetTokenCookie("access_token", result.AccessToken, result.AccessTokenExpiry);
            SetTokenCookie("refresh_token", result.RefreshToken, System.DateTime.UtcNow.AddDays(7));

            return Ok(new { Message = "Cấp lại Token thành công" });
        }
        catch (System.Exception ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
    }

    /// <summary>
    /// Logout: xóa RefreshToken trong DB và xóa Cookies.
    /// </summary>
    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
                return Unauthorized(new { Message = "Không tìm thấy thông tin user." });

            var userId = int.Parse(userIdStr);
            await _jwtTokenService.RevokeRefreshTokenAsync(userId);

            Response.Cookies.Delete("access_token");
            Response.Cookies.Delete("refresh_token");

            return Ok(new { Message = "Đăng xuất thành công. Token đã bị xóa." });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    private void SetTokenCookie(string key, string token, System.DateTime expires)
    {
        var cookieOptions = new Microsoft.AspNetCore.Http.CookieOptions
        {
            HttpOnly = true,
            Expires = expires,
            Secure = true, // Chỉ truyền qua HTTPS
            SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict
        };
        Response.Cookies.Append(key, token, cookieOptions);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
    {
        try
        {
            var result = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(Login), new { id = result.UserId }, result);
        }
        catch (System.Exception ex)
        {
            return BadRequest(new 
            { 
                Message = ex.Message,
                InnerError = ex.InnerException?.Message
            });
        }
    }

    [Authorize]
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        // Token hợp lệ thì mới vào được hàm này.
        // User.FindFirstValue sẽ lấy các Claims mà ta đã setup ở Payload lúc tạo Token!
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.FindFirstValue(ClaimTypes.Name);
        var role = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new
        {
            Message = "🎉 Chúc mừng! Bạn đã dùng Token truy cập thành công!",
            UserId = userId,
            Username = username,
            Role = role
        });
    }
}
