using System.Collections.Generic;

namespace SmartQRCoffee.Repositories.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? IconUrl { get; set; } // <-- Thêm IconUrl để map với icon ngoài UI
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
