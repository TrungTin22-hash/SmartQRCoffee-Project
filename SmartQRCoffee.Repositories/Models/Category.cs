using System.Collections.Generic;

namespace SmartQRCoffee.Repositories.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
