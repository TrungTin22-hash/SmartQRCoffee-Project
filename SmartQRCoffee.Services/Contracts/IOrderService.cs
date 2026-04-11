using System.Threading.Tasks;
using SmartQRCoffee.Services.DTOs;

namespace SmartQRCoffee.Services.Contracts;

public interface IOrderService
{
    Task<OrderResponseDto> SubmitOrderAsync(CreateOrderDto dto);
    Task<OrderStatusUpdateResponseDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto dto);
}
