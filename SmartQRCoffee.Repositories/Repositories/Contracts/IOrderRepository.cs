using SmartQRCoffee.Repositories.Models;

namespace SmartQRCoffee.Repositories.Repositories.Contracts;

public interface IOrderRepository
{
    Task<List<Order>> GetOrdersAsync();
    Task<Order?> GetOrderAsync(int id);
    Task<Order> AddOrderAsync(Order order);
    Task<Order> UpdateOrderAsync(Order order);
    Task<Order> DeleteOrderAsync(int id);
}
