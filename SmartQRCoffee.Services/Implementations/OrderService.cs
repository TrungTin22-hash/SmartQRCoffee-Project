using System;
using System.Linq;
using System.Threading.Tasks;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using SmartQRCoffee.Repositories.Models;
using SmartQRCoffee.Services.Contracts;
using SmartQRCoffee.Services.DTOs;

namespace SmartQRCoffee.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ITableRepository _tableRepository;
    private readonly IProductRepository _productRepository;
    private readonly INotificationService _notificationService;

    public OrderService(
        IOrderRepository orderRepository,
        ITableRepository tableRepository,
        IProductRepository productRepository,
        INotificationService notificationService)
    {
        _orderRepository = orderRepository;
        _tableRepository = tableRepository;
        _productRepository = productRepository;
        _notificationService = notificationService;
    }

    public async Task<OrderResponseDto> SubmitOrderAsync(CreateOrderDto dto)
    {
        // 1. Validate session token
        var table = await _tableRepository.GetTableAsync(dto.TableId);

        if (table == null || table.SessionToken != dto.SessionToken || !table.IsActive)
        {
            throw new Exception("Invalid session token or table not active.");
        }

        // 2 & 3. Verify availability & Calculate Total Amount
        decimal totalAmount = 0;
        var orderItems = new System.Collections.Generic.List<OrderDetail>();

        foreach (var itemDto in dto.Items)
        {
            var product = await _productRepository.GetProductAsync(itemDto.ProductId);
            
            if (product == null || product.IsDisabled || product.Stock_Quantity < itemDto.Quantity)
            {
                throw new Exception($"Product {itemDto.ProductId} is not available.");
            }

            // Calculate options (assuming options are pre-loaded or we can look them up)
            decimal optionsTotal = 0;
            if (itemDto.Options != null && itemDto.Options.Any() && product.Options != null)
            {
                var selectedOptions = product.Options.Where(o => itemDto.Options.Contains(o.ProductOptionId));
                optionsTotal = selectedOptions.Sum(o => o.PriceAdjustment);
            }

            var itemPrice = product.Price + optionsTotal;
            totalAmount += itemPrice * itemDto.Quantity;

            orderItems.Add(new OrderDetail
            {
                ProductId = product.ProductId,
                Quantity = itemDto.Quantity,
                UnitPrice = itemPrice // Note in OrderDetail
            });
        }

        // 4. Persist Order
        var order = new Order
        {
            TableId = dto.TableId,
            SessionToken = dto.SessionToken,
            TotalAmount = totalAmount,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow,
            OrderDetails = orderItems,
            Payment = new Payment
            {
                PaymentMethod = dto.PaymentMethod,
                Amount = totalAmount,
                Status = "Pending",
                PaymentTime = DateTime.UtcNow
            }
        };

        var savedOrder = await _orderRepository.AddOrderAsync(order);

        // 5. Send Notification via SignalR Abstraction
        await _notificationService.NotifyKitchenNewOrderAsync(savedOrder);

        return new OrderResponseDto
        {
            OrderId = $"ORD-{savedOrder.OrderId}",
            TotalAmount = savedOrder.TotalAmount,
            Status = savedOrder.Status,
            Message = "Order placed successfully. The kitchen is preparing your drinks!"
        };
    }

    public async Task<OrderStatusUpdateResponseDto> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusDto dto)
    {
        var order = await _orderRepository.GetOrderAsync(orderId);
        if (order == null)
        {
            throw new Exception("Order not found.");
        }

        order.Status = dto.NewStatus;
        await _orderRepository.UpdateOrderAsync(order);

        // Trigger SignalR Notification
        await _notificationService.NotifyCustomerOrderStatusChangedAsync(order.TableId, dto.NewStatus);

        return new OrderStatusUpdateResponseDto
        {
            OrderId = $"ORD-{order.OrderId}",
            CurrentStatus = order.Status,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
