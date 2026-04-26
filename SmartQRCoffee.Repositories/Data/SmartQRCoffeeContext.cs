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
        // Connection string for Supabase PostgreSQL (IPv4 Session Pooler)
        optionsBuilder.UseNpgsql("Host=aws-1-ap-northeast-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.xaycpyxshemncmhlerwa;Password=Smartqrcoffee123");
    }
}
