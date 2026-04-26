using System.Collections.Generic;

namespace SmartQRCoffee.Repositories.Models;

public class Product
{
    public int ProductId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } // <-- Thêm mô tả ("Đậm đà kiểu truyền thống...")
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsFeatured { get; set; } // <-- Thêm Cờ "Gợi ý cho bạn"
    public bool IsNew { get; set; } // <-- Thêm Cờ "Món Mới" cho slide banner
    public int Stock_Quantity { get; set; }
    public bool IsDisabled { get; set; }
    public virtual Category Category { get; set; } = null!;
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual ICollection<ProductOption> Options { get; set; } = new List<ProductOption>();
}
