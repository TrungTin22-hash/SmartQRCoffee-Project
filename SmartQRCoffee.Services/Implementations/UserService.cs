using System;
using System.Threading.Tasks;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using SmartQRCoffee.Repositories.Models;
using SmartQRCoffee.Services.Contracts;
using SmartQRCoffee.Services.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartQRCoffee.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Username = dto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            RoleId = dto.RoleId,
            IsActive = true
        };

        var createdUser = await _userRepository.AddUserAsync(user);

        return new UserDto
        {
            UserId = createdUser.UserId,
            Username = createdUser.Username,
            RoleId = createdUser.RoleId,
            RoleName = createdUser.Role?.RoleName ?? string.Empty
        };
    }

    public async Task<AuthResponseDto> LoginAsync(UserLoginDto dto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(dto.Username);
        
        if (user == null || !user.IsActive || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            throw new Exception("Invalid username or password.");
        }

        var userDto = new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            RoleId = user.RoleId,
            RoleName = user.Role?.RoleName ?? string.Empty
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var keyVal = _configuration["Jwt:Key"] ?? throw new Exception("JWT Key not configured");
        var key = Encoding.UTF8.GetBytes(keyVal);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, userDto.RoleName)
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthResponseDto
        {
            User = userDto,
            AccessToken = tokenHandler.WriteToken(token)
        };
    }
}
