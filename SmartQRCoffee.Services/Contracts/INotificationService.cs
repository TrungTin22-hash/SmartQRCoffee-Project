using System.Threading.Tasks;

namespace SmartQRCoffee.Services.Contracts;

public interface INotificationService
{
    Task NotifyKitchenNewOrderAsync(object orderPayload);
    Task NotifyCustomerOrderStatusChangedAsync(int tableId, string newStatus);
}
