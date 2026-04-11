using SmartQRCoffee.Repositories.Data;
using SmartQRCoffee.Repositories.Models;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace SmartQRCoffee.Repositories.Repositories.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly SmartQRCoffeeContext _context;

    public CategoryRepository(SmartQRCoffeeContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> GetCategoryAsync(int id)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
    }

    public async Task<Category> AddCategoryAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> UpdateCategoryAsync(Category category)
    {
        var categoryInDb = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);
        if (categoryInDb != null)
        {
            categoryInDb.Name = category.Name;
            
            _context.Categories.Update(categoryInDb);
            await _context.SaveChangesAsync();
        }
        return categoryInDb;
    }

    public async Task<Category> DeleteCategoryAsync(int id)
    {
        var categoryInDb = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        if (categoryInDb != null)
        {
            _context.Categories.Remove(categoryInDb);
            await _context.SaveChangesAsync();
        }
        return categoryInDb;
    }
}
