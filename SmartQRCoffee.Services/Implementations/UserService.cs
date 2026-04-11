using System;
using System.Threading.Tasks;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using SmartQRCoffee.Repositories.Models;
using SmartQRCoffee.Services.Contracts;
using SmartQRCoffee.Services.DTOs;

namespace SmartQRCoffee.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Username = dto.Username,
            PasswordHash = dto.Password, // In a real app, hash the password
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

    public async Task<UserDto> LoginAsync(UserLoginDto dto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(dto.Username);
        
        if (user == null || user.PasswordHash != dto.Password || !user.IsActive)
        {
            throw new Exception("Invalid username or password.");
        }

        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            RoleId = user.RoleId,
            RoleName = user.Role?.RoleName ?? string.Empty
        };
    }
}
