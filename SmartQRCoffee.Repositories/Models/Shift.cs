using System;

namespace SmartQRCoffee.Repositories.Models;

public class Shift
{
    public int ShiftId { get; set; }
    public int UserId { get; set; }
    public DateTime StartTime { get; set; } = DateTime.UtcNow;
    public DateTime? EndTime { get; set; }
    public decimal StartingCash { get; set; }
    public decimal ExpectedCash { get; set; }
    public decimal ActualCash { get; set; }
    public decimal Discrepancy { get; set; }
    public string Status { get; set; } = "Open";
    public virtual User User { get; set; } = null!;
}
