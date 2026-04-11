using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartQRCoffee.Repositories.Data;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using SmartQRCoffee.Repositories.Repositories.Implementations;
using SmartQRCoffee.Services.Contracts;
using SmartQRCoffee.Services.Implementations;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<SmartQRCoffeeContext>();

// Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Register Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITableService, TableService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Mock Notification Service (To be replaced with actual SignalR Hub Context later)
builder.Services.AddSingleton<INotificationService, MockNotificationService>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Mock implementation
public class MockNotificationService : INotificationService
{
    public Task NotifyCustomerOrderStatusChangedAsync(int tableId, string newStatus)
    {
        System.Console.WriteLine($"[MockSignalR] Table {tableId} order status changed to {newStatus}");
        return Task.CompletedTask;
    }

    public Task NotifyKitchenNewOrderAsync(object orderPayload)
    {
        System.Console.WriteLine($"[MockSignalR] Kitchen received new order: {System.Text.Json.JsonSerializer.Serialize(orderPayload)}");
        return Task.CompletedTask;
    }
}
