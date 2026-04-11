using SmartQRCoffee.Repositories.Data;
using SmartQRCoffee.Repositories.Models;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace SmartQRCoffee.Repositories.Repositories.Implementations;

public class OrderDetailRepository : IOrderDetailRepository
{
    private readonly SmartQRCoffeeContext _context;

    public OrderDetailRepository(SmartQRCoffeeContext context)
    {
        _context = context;
    }

    public async Task<List<OrderDetail>> GetOrderDetailsAsync()
    {
        return await _context.OrderDetails.ToListAsync();
    }

    public async Task<OrderDetail?> GetOrderDetailAsync(int id)
    {
        return await _context.OrderDetails.FirstOrDefaultAsync(od => od.OrderDetailId == id);
    }

    public async Task<OrderDetail> AddOrderDetailAsync(OrderDetail orderDetail)
    {
        await _context.OrderDetails.AddAsync(orderDetail);
        await _context.SaveChangesAsync();
        return orderDetail;
    }

    public async Task<OrderDetail> UpdateOrderDetailAsync(OrderDetail orderDetail)
    {
        var orderDetailInDb = await _context.OrderDetails.FirstOrDefaultAsync(od => od.OrderDetailId == orderDetail.OrderDetailId);
        if (orderDetailInDb != null)
        {
            orderDetailInDb.OrderId = orderDetail.OrderId;
            orderDetailInDb.ProductId = orderDetail.ProductId;
            orderDetailInDb.Quantity = orderDetail.Quantity;
            orderDetailInDb.UnitPrice = orderDetail.UnitPrice;
            
            _context.OrderDetails.Update(orderDetailInDb);
            await _context.SaveChangesAsync();
        }
        return orderDetailInDb;
    }

    public async Task<OrderDetail> DeleteOrderDetailAsync(int id)
    {
        var orderDetailInDb = await _context.OrderDetails.FirstOrDefaultAsync(od => od.OrderDetailId == id);
        if (orderDetailInDb != null)
        {
            _context.OrderDetails.Remove(orderDetailInDb);
            await _context.SaveChangesAsync();
        }
        return orderDetailInDb;
    }
}
