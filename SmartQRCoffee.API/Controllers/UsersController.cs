using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartQRCoffee.Services.Contracts;
using SmartQRCoffee.Services.DTOs;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartQRCoffee.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IConfiguration _configuration;

    public UsersController(IUserService userService, IJwtTokenService jwtTokenService, IConfiguration configuration)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        try
        {
            var result = await _jwtTokenService.LoginAsync(dto);

            SetTokenCookie(GetAccessTokenCookieName(), result.AccessToken, result.AccessTokenExpiry);
            SetTokenCookie(GetRefreshTokenCookieName(), result.RefreshToken, DateTime.UtcNow.AddDays(GetRefreshTokenExpiryDays()));

            return Ok(new { Message = "Đăng nhập thành công", User = result.User });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("exchange-token")]
    public async Task<IActionResult> ExchangeToken()
    {
        try
        {
            var refreshToken = Request.Cookies[GetRefreshTokenCookieName()];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new { Message = "Không tìm thấy Refresh Token trong Cookie." });
            }

            var dto = new ExchangeTokenDto { RefreshToken = refreshToken };
            var result = await _jwtTokenService.ExchangeTokenAsync(dto);

            SetTokenCookie(GetAccessTokenCookieName(), result.AccessToken, result.AccessTokenExpiry);
            SetTokenCookie(GetRefreshTokenCookieName(), result.RefreshToken, DateTime.UtcNow.AddDays(GetRefreshTokenExpiryDays()));

            return Ok(new { Message = "Cấp lại Token thành công" });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr))
            {
                return Unauthorized(new { Message = "Không tìm thấy thông tin user." });
            }

            var userId = int.Parse(userIdStr);
            await _jwtTokenService.RevokeRefreshTokenAsync(userId);

            Response.Cookies.Delete(GetAccessTokenCookieName());
            Response.Cookies.Delete(GetRefreshTokenCookieName());

            return Ok(new { Message = "Đăng xuất thành công. Token đã bị xóa." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
    {
        try
        {
            var result = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(Login), new { id = result.UserId }, result);
        }
        catch (Exception ex)
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

    private void SetTokenCookie(string key, string token, DateTime expires)
    {
        var sameSite = ParseSameSiteMode(_configuration["JwtConfig:Cookie:SameSite"]);
        var securePolicy = ParseSecurePolicy(_configuration["JwtConfig:Cookie:SecurePolicy"]);
        var secure = securePolicy switch
        {
            CookieSecurePolicy.Always => true,
            CookieSecurePolicy.None => false,
            _ => Request.IsHttps
        };

        var cookieOptions = new CookieOptions
        {
            HttpOnly = _configuration.GetValue("JwtConfig:Cookie:HttpOnly", true),
            Expires = expires,
            Secure = secure,
            SameSite = sameSite,
            IsEssential = true
        };

        Response.Cookies.Append(key, token, cookieOptions);
    }

    private string GetAccessTokenCookieName() => _configuration["JwtConfig:Cookie:AccessTokenName"] ?? "access_token";
    private string GetRefreshTokenCookieName() => _configuration["JwtConfig:Cookie:RefreshTokenName"] ?? "refresh_token";
    private int GetRefreshTokenExpiryDays() => _configuration.GetValue<int>("JwtConfig:RefreshTokenExpirationDays");

    private static SameSiteMode ParseSameSiteMode(string? value) => value?.ToLowerInvariant() switch
    {
        "none" => SameSiteMode.None,
        "lax" => SameSiteMode.Lax,
        _ => SameSiteMode.Strict
    };

    private static CookieSecurePolicy ParseSecurePolicy(string? value) => value?.ToLowerInvariant() switch
    {
        "always" => CookieSecurePolicy.Always,
        "none" => CookieSecurePolicy.None,
        _ => CookieSecurePolicy.SameAsRequest
    };
}
