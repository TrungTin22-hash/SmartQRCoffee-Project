using SmartQRCoffee.Repositories.Models;

namespace SmartQRCoffee.Repositories.Repositories.Contracts;

public interface IOrderDetailRepository
{
    Task<List<OrderDetail>> GetOrderDetailsAsync();
    Task<OrderDetail?> GetOrderDetailAsync(int id);
    Task<OrderDetail> AddOrderDetailAsync(OrderDetail orderDetail);
    Task<OrderDetail> UpdateOrderDetailAsync(OrderDetail orderDetail);
    Task<OrderDetail> DeleteOrderDetailAsync(int id);
}
