using System;
using System.Collections.Generic;

namespace SmartQRCoffee.Repositories.Models;

public class Order
{
    public int OrderId { get; set; }
    public int TableId { get; set; }
    public string? SessionToken { get; set; }
    public string Status { get; set; } = "Pending";
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public virtual Table Table { get; set; } = null!;
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual Payment Payment { get; set; } = null!;
}
