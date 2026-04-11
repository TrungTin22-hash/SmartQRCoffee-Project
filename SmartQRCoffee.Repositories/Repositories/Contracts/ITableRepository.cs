using SmartQRCoffee.Repositories.Models;

namespace SmartQRCoffee.Repositories.Repositories.Contracts;

public interface ITableRepository
{
    Task<List<Table>> GetTablesAsync();
    Task<Table?> GetTableAsync(int id);
    Task<Table> AddTableAsync(Table table);
    Task<Table> UpdateTableAsync(Table table);
    Task<Table> DeleteTableAsync(int id);
}
