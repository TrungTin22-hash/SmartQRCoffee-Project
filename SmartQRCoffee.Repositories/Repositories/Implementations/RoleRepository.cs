using SmartQRCoffee.Repositories.Data;
using SmartQRCoffee.Repositories.Models;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace SmartQRCoffee.Repositories.Repositories.Implementations;

public class RoleRepository : IRoleRepository
{
    private readonly SmartQRCoffeeContext _context;

    public RoleRepository(SmartQRCoffeeContext context)
    {
        _context = context;
    }

    public async Task<List<Role>> GetRolesAsync()
    {
        return await _context.Roles.ToListAsync();
    }

    public async Task<Role?> GetRoleAsync(int id)
    {
        return await _context.Roles.FirstOrDefaultAsync(s => s.RoleId == id);
    }

    public async Task<Role> AddRoleAsync(Role role)
    {
        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task<Role> UpdateRoleAsync(Role role)
    {
        var roleInDb = await _context.Roles.FirstOrDefaultAsync(s => s.RoleId == role.RoleId);
        if (roleInDb != null)
        {
            roleInDb.RoleName = role.RoleName;
            _context.Roles.Update(roleInDb);
            await _context.SaveChangesAsync();
        }
        return roleInDb;
    }

    public async Task<Role> DeleteRoleAsync(int id)
    {
        var roleInDb = await _context.Roles.FirstOrDefaultAsync(s => s.RoleId == id);
        if (roleInDb != null)
        {
            _context.Roles.Remove(roleInDb);
            await _context.SaveChangesAsync();
        }
        return roleInDb;
    }
}
