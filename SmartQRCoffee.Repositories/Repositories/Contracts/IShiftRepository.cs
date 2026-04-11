using SmartQRCoffee.Repositories.Models;

namespace SmartQRCoffee.Repositories.Repositories.Contracts;

public interface IShiftRepository
{
    Task<List<Shift>> GetShiftsAsync();
    Task<Shift?> GetShiftAsync(int id);
    Task<Shift> AddShiftAsync(Shift shift);
    Task<Shift> UpdateShiftAsync(Shift shift);
    Task<Shift> DeleteShiftAsync(int id);
}
