using Microsoft.AspNetCore.Mvc;
using SmartQRCoffee.Services.Contracts;
using SmartQRCoffee.Services.DTOs;
using System.Threading.Tasks;

namespace SmartQRCoffee.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitOrder([FromBody] CreateOrderDto dto)
    {
        try
        {
            var result = await _orderService.SubmitOrderAsync(dto);
            return Created("", result);
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPatch("{orderId}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto dto)
    {
        try
        {
            var result = await _orderService.UpdateOrderStatusAsync(orderId, dto);
            return Ok(result);
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
