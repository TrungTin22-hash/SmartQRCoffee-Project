namespace SmartQRCoffee.Repositories.Models;

public class ProductOption
{
    public int ProductOptionId { get; set; }
    public int ProductId { get; set; }
    public string Name { get; set; } = null!;
    public decimal PriceAdjustment { get; set; }
    
    public virtual Product Product { get; set; } = null!;
}
