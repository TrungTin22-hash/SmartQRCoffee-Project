using SmartQRCoffee.Repositories.Data;
using SmartQRCoffee.Repositories.Models;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace SmartQRCoffee.Repositories.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly SmartQRCoffeeContext _context;

    public UserRepository(SmartQRCoffeeContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User> AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
        if (userInDb != null)
        {
            userInDb.Username = user.Username;
            userInDb.PasswordHash = user.PasswordHash;
            userInDb.RoleId = user.RoleId;
            userInDb.IsActive = user.IsActive;
            userInDb.RefreshToken = user.RefreshToken;
            userInDb.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;
            
            _context.Users.Update(userInDb);
            await _context.SaveChangesAsync();
        }
        return userInDb;
    }

    public async Task<User> DeleteUserAsync(int id)
    {
        var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (userInDb != null)
        {
            _context.Users.Remove(userInDb);
            await _context.SaveChangesAsync();
        }
        return userInDb;
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
    }
}

