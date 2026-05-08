using SmartQRCoffee.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartQRCoffee.Repositories.Data;

public class SmartQRCoffeeContext : DbContext
{
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Table> Tables { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderDetail> OrderDetails { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<Shift> Shifts { get; set; }
    public virtual DbSet<ProductOption> ProductOptions { get; set; }

    public SmartQRCoffeeContext()
    {
    }

    public SmartQRCoffeeContext(DbContextOptions<SmartQRCoffeeContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            throw new InvalidOperationException("SmartQRCoffeeContext must be configured via dependency injection.");
        }
    }
}
