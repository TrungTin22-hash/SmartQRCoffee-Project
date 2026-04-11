namespace SmartQRCoffee.Services.DTOs;

public class UserLoginDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class CreateUserDto
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int RoleId { get; set; }
}

public class UserDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
}
