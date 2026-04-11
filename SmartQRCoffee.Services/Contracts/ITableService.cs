using System.Threading.Tasks;
using SmartQRCoffee.Services.DTOs;

namespace SmartQRCoffee.Services.Contracts;

public interface ITableService
{
    Task<TableMenuResponseDto> ValidateTableAndGetMenuAsync(string token);
}
