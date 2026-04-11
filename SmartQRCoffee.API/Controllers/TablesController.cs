using Microsoft.AspNetCore.Mvc;
using SmartQRCoffee.Services.Contracts;
using System.Threading.Tasks;

namespace SmartQRCoffee.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TablesController : ControllerBase
{
    private readonly ITableService _tableService;

    public TablesController(ITableService tableService)
    {
        _tableService = tableService;
    }

    [HttpGet("{token}/menu")]
    public async Task<IActionResult> ValidateTableAndGetMenu(string token)
    {
        try
        {
            var result = await _tableService.ValidateTableAndGetMenuAsync(token);
            return Ok(result);
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
