using SmartQRCoffee.Repositories.Data;
using SmartQRCoffee.Repositories.Models;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace SmartQRCoffee.Repositories.Repositories.Implementations;

public class ShiftRepository : IShiftRepository
{
    private readonly SmartQRCoffeeContext _context;

    public ShiftRepository(SmartQRCoffeeContext context)
    {
        _context = context;
    }

    public async Task<List<Shift>> GetShiftsAsync()
    {
        return await _context.Shifts.ToListAsync();
    }

    public async Task<Shift?> GetShiftAsync(int id)
    {
        return await _context.Shifts.FirstOrDefaultAsync(s => s.ShiftId == id);
    }

    public async Task<Shift> AddShiftAsync(Shift shift)
    {
        await _context.Shifts.AddAsync(shift);
        await _context.SaveChangesAsync();
        return shift;
    }

    public async Task<Shift> UpdateShiftAsync(Shift shift)
    {
        var shiftInDb = await _context.Shifts.FirstOrDefaultAsync(s => s.ShiftId == shift.ShiftId);
        if (shiftInDb != null)
        {
            shiftInDb.UserId = shift.UserId;
            shiftInDb.StartTime = shift.StartTime;
            shiftInDb.EndTime = shift.EndTime;
            shiftInDb.StartingCash = shift.StartingCash;
            shiftInDb.ExpectedCash = shift.ExpectedCash;
            shiftInDb.ActualCash = shift.ActualCash;
            shiftInDb.Discrepancy = shift.Discrepancy;
            shiftInDb.Status = shift.Status;
            
            _context.Shifts.Update(shiftInDb);
            await _context.SaveChangesAsync();
        }
        return shiftInDb;
    }

    public async Task<Shift> DeleteShiftAsync(int id)
    {
        var shiftInDb = await _context.Shifts.FirstOrDefaultAsync(s => s.ShiftId == id);
        if (shiftInDb != null)
        {
            _context.Shifts.Remove(shiftInDb);
            await _context.SaveChangesAsync();
        }
        return shiftInDb;
    }
}
