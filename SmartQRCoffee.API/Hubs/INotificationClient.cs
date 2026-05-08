using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartQRCoffee.API.Hubs;

public interface INotificationClient
{
    Task ReceiveNewOrder(NewOrderNotificationDto payload);
    Task ReceiveOrderStatusChanged(OrderStatusChangedNotificationDto payload);
}

public class NewOrderNotificationDto
{
    public int OrderId { get; set; }
    public int TableId { get; set; }
    public string? SessionToken { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public List<NewOrderItemNotificationDto> Items { get; set; } = new();
}

public class NewOrderItemNotificationDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public List<int> Options { get; set; } = new();
    public string? Note { get; set; }
}

public class OrderStatusChangedNotificationDto
{
    public int TableId { get; set; }
    public string NewStatus { get; set; } = string.Empty;
}
