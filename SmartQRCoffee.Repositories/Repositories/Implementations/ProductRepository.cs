using SmartQRCoffee.Repositories.Data;
using SmartQRCoffee.Repositories.Models;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace SmartQRCoffee.Repositories.Repositories.Implementations;

public class ProductRepository : IProductRepository
{
    private readonly SmartQRCoffeeContext _context;

    public ProductRepository(SmartQRCoffeeContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
    }

    public async Task<Product> AddProductAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        var productInDb = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == product.ProductId);
        if (productInDb != null)
        {
            productInDb.CategoryId = product.CategoryId;
            productInDb.Name = product.Name;
            productInDb.Price = product.Price;
            productInDb.Stock_Quantity = product.Stock_Quantity;
            productInDb.IsDisabled = product.IsDisabled;
            
            _context.Products.Update(productInDb);
            await _context.SaveChangesAsync();
        }
        return productInDb;
    }

    public async Task<Product> DeleteProductAsync(int id)
    {
        var productInDb = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        if (productInDb != null)
        {
            _context.Products.Remove(productInDb);
            await _context.SaveChangesAsync();
        }
        return productInDb;
    }
}
