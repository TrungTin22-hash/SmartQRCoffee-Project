using System;
using System.Collections.Generic;

namespace SmartQRCoffee.Services.DTOs;

public class OrderItemRequestDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public List<int> Options { get; set; } = new List<int>();
    public string? Note { get; set; }
}

public class CreateOrderDto
{
    public int TableId { get; set; }
    public string SessionToken { get; set; } = null!;
    public string PaymentMethod { get; set; } = null!;
    public List<OrderItemRequestDto> Items { get; set; } = new List<OrderItemRequestDto>();
}

public class OrderResponseDto
{
    public string OrderId { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = null!;
    public string Message { get; set; } = null!;
}

public class UpdateOrderStatusDto
{
    public string NewStatus { get; set; } = null!;
}

public class OrderStatusUpdateResponseDto
{
    public string OrderId { get; set; } = null!;
    public string CurrentStatus { get; set; } = null!;
    public DateTime UpdatedAt { get; set; }
}
