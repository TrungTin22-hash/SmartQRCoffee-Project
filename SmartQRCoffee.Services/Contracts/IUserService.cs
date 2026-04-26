using System.Threading.Tasks;
using SmartQRCoffee.Services.DTOs;

namespace SmartQRCoffee.Services.Contracts;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(CreateUserDto dto);
    Task<AuthResponseDto> LoginAsync(UserLoginDto dto);
}
