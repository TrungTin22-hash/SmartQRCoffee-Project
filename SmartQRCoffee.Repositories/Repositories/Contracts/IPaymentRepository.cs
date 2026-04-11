using SmartQRCoffee.Repositories.Models;

namespace SmartQRCoffee.Repositories.Repositories.Contracts;

public interface IPaymentRepository
{
    Task<List<Payment>> GetPaymentsAsync();
    Task<Payment?> GetPaymentAsync(int id);
    Task<Payment> AddPaymentAsync(Payment payment);
    Task<Payment> UpdatePaymentAsync(Payment payment);
    Task<Payment> DeletePaymentAsync(int id);
}
