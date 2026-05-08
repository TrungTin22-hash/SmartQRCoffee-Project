using Microsoft.AspNetCore.SignalR;
using SmartQRCoffee.API.Hubs;
using SmartQRCoffee.Services.Contracts;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartQRCoffee.API.Services;

public class SignalRNotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
    private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public SignalRNotificationService(IHubContext<NotificationHub, INotificationClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyKitchenNewOrderAsync(object orderPayload)
    {
        var payloadJson = JsonSerializer.Serialize(orderPayload);
        var payload = JsonSerializer.Deserialize<NewOrderNotificationDto>(payloadJson, _serializerOptions)
            ?? new NewOrderNotificationDto();

        payload.Items ??= new List<NewOrderItemNotificationDto>();

        await _hubContext.Clients.Group(NotificationHub.KitchenGroupName)
            .ReceiveNewOrder(payload);
    }

    public async Task NotifyCustomerOrderStatusChangedAsync(int tableId, string newStatus)
    {
        var payload = new OrderStatusChangedNotificationDto
        {
            TableId = tableId,
            NewStatus = newStatus
        };

        await _hubContext.Clients.Group(NotificationHub.GetTableGroupName(tableId))
            .ReceiveOrderStatusChanged(payload);
    }
}
