using SmartQRCoffee.Repositories.Models;

namespace SmartQRCoffee.Repositories.Repositories.Contracts;

public interface ICategoryRepository
{
    Task<List<Category>> GetCategoriesAsync();
    Task<Category?> GetCategoryAsync(int id);
    Task<Category> AddCategoryAsync(Category category);
    Task<Category> UpdateCategoryAsync(Category category);
    Task<Category> DeleteCategoryAsync(int id);
}
