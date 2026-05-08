using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SmartQRCoffee.API.Hubs;

public class NotificationHub : Hub<INotificationClient>
{
    public const string KitchenGroupName = "kitchen";

    public Task JoinKitchen()
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, KitchenGroupName);
    }

    public Task LeaveKitchen()
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, KitchenGroupName);
    }

    public Task JoinTableGroup(int tableId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, GetTableGroupName(tableId));
    }

    public Task LeaveTableGroup(int tableId)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, GetTableGroupName(tableId));
    }

    public static string GetTableGroupName(int tableId) => $"table-{tableId}";
}
