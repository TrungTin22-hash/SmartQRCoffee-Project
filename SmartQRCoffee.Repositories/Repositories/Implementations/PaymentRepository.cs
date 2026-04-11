using SmartQRCoffee.Repositories.Data;
using SmartQRCoffee.Repositories.Models;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace SmartQRCoffee.Repositories.Repositories.Implementations;

public class PaymentRepository : IPaymentRepository
{
    private readonly SmartQRCoffeeContext _context;

    public PaymentRepository(SmartQRCoffeeContext context)
    {
        _context = context;
    }

    public async Task<List<Payment>> GetPaymentsAsync()
    {
        return await _context.Payments.ToListAsync();
    }

    public async Task<Payment?> GetPaymentAsync(int id)
    {
        return await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == id);
    }

    public async Task<Payment> AddPaymentAsync(Payment payment)
    {
        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();
        return payment;
    }

    public async Task<Payment> UpdatePaymentAsync(Payment payment)
    {
        var paymentInDb = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == payment.PaymentId);
        if (paymentInDb != null)
        {
            paymentInDb.OrderId = payment.OrderId;
            paymentInDb.PaymentMethod = payment.PaymentMethod;
            paymentInDb.Amount = payment.Amount;
            paymentInDb.Status = payment.Status;
            paymentInDb.PaymentTime = payment.PaymentTime;
            
            _context.Payments.Update(paymentInDb);
            await _context.SaveChangesAsync();
        }
        return paymentInDb;
    }

    public async Task<Payment> DeletePaymentAsync(int id)
    {
        var paymentInDb = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == id);
        if (paymentInDb != null)
        {
            _context.Payments.Remove(paymentInDb);
            await _context.SaveChangesAsync();
        }
        return paymentInDb;
    }
}
