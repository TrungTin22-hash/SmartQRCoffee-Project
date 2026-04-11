using System.Collections.Generic;

namespace SmartQRCoffee.Repositories.Models;

public class Table
{
    public int TableId { get; set; }
    public string TableName { get; set; } = null!;
    public string QRCode { get; set; } = null!;
    public string? SessionToken { get; set; }
    public bool IsOccupied { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
