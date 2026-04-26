using System.Collections.Generic;

namespace SmartQRCoffee.Repositories.Models;

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public int RoleId { get; set; }
    public bool IsActive { get; set; } = true;
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public virtual Role Role { get; set; } = null!;
    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
