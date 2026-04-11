using SmartQRCoffee.Repositories.Models;

namespace SmartQRCoffee.Repositories.Repositories.Contracts;

public interface IRoleRepository
{
    Task<List<Role>> GetRolesAsync();
    Task<Role?> GetRoleAsync(int id);
    Task<Role> AddRoleAsync(Role role);
    Task<Role> UpdateRoleAsync(Role role);
    Task<Role> DeleteRoleAsync(int id);
}
