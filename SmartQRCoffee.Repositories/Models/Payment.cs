using System;

namespace SmartQRCoffee.Repositories.Models;

public class Payment
{
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public string PaymentMethod { get; set; } = null!;
    public decimal Amount { get; set; }
    public string Status { get; set; } = "Success";
    public DateTime PaymentTime { get; set; } = DateTime.UtcNow;
    public virtual Order Order { get; set; } = null!;
}
