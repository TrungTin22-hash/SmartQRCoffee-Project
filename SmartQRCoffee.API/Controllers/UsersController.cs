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

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        try
        {
            var result = await _userService.LoginAsync(dto);
            return Ok(result);
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
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
