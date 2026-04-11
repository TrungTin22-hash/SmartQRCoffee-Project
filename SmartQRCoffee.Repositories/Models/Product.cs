using System.Collections.Generic;

namespace SmartQRCoffee.Repositories.Models;

public class Product
{
    public int ProductId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int Stock_Quantity { get; set; }
    public bool IsDisabled { get; set; }
    public virtual Category Category { get; set; } = null!;
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual ICollection<ProductOption> Options { get; set; } = new List<ProductOption>();
}
