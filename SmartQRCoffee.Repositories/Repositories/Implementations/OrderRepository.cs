using SmartQRCoffee.Repositories.Data;
using SmartQRCoffee.Repositories.Models;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace SmartQRCoffee.Repositories.Repositories.Implementations;

public class OrderRepository : IOrderRepository
{
    private readonly SmartQRCoffeeContext _context;

    public OrderRepository(SmartQRCoffeeContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order?> GetOrderAsync(int id)
    {
        return await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
    }

    public async Task<Order> AddOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> UpdateOrderAsync(Order order)
    {
        var orderInDb = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == order.OrderId);
        if (orderInDb != null)
        {
            orderInDb.TableId = order.TableId;
            orderInDb.Status = order.Status;
            orderInDb.TotalAmount = order.TotalAmount;
            orderInDb.CreatedAt = order.CreatedAt;
            
            _context.Orders.Update(orderInDb);
            await _context.SaveChangesAsync();
        }
        return orderInDb;
    }

    public async Task<Order> DeleteOrderAsync(int id)
    {
        var orderInDb = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
        if (orderInDb != null)
        {
            _context.Orders.Remove(orderInDb);
            await _context.SaveChangesAsync();
        }
        return orderInDb;
    }
}
