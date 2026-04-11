using SmartQRCoffee.Repositories.Data;
using SmartQRCoffee.Repositories.Models;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace SmartQRCoffee.Repositories.Repositories.Implementations;

public class TableRepository : ITableRepository
{
    private readonly SmartQRCoffeeContext _context;

    public TableRepository(SmartQRCoffeeContext context)
    {
        _context = context;
    }

    public async Task<List<Table>> GetTablesAsync()
    {
        return await _context.Tables.ToListAsync();
    }

    public async Task<Table?> GetTableAsync(int id)
    {
        return await _context.Tables.FirstOrDefaultAsync(t => t.TableId == id);
    }

    public async Task<Table> AddTableAsync(Table table)
    {
        await _context.Tables.AddAsync(table);
        await _context.SaveChangesAsync();
        return table;
    }

    public async Task<Table> UpdateTableAsync(Table table)
    {
        var tableInDb = await _context.Tables.FirstOrDefaultAsync(t => t.TableId == table.TableId);
        if (tableInDb != null)
        {
            tableInDb.TableName = table.TableName;
            tableInDb.QRCode = table.QRCode;
            tableInDb.IsActive = table.IsActive;
            
            _context.Tables.Update(tableInDb);
            await _context.SaveChangesAsync();
        }
        return tableInDb;
    }

    public async Task<Table> DeleteTableAsync(int id)
    {
        var tableInDb = await _context.Tables.FirstOrDefaultAsync(t => t.TableId == id);
        if (tableInDb != null)
        {
            _context.Tables.Remove(tableInDb);
            await _context.SaveChangesAsync();
        }
        return tableInDb;
    }
}
