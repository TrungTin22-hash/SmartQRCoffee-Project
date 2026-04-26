using SmartQRCoffee.Repositories.Models;

namespace SmartQRCoffee.Repositories.Repositories.Contracts;

public interface IUserRepository
{
    Task<List<User>> GetUsersAsync();
    Task<User?> GetUserAsync(int id);
    Task<User?> GetUserByUsernameAsync(string username);
    Task<User> AddUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<User> DeleteUserAsync(int id);
    Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
}
