using System;
using System.Linq;
using System.Threading.Tasks;
using SmartQRCoffee.Repositories.Repositories.Contracts;
using SmartQRCoffee.Services.Contracts;
using SmartQRCoffee.Services.DTOs;

namespace SmartQRCoffee.Services.Implementations;

public class TableService : ITableService
{
    private readonly ITableRepository _tableRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;

    public TableService(
        ITableRepository tableRepository,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository)
    {
        _tableRepository = tableRepository;
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }

    public async Task<TableMenuResponseDto> ValidateTableAndGetMenuAsync(string token)
    {
        var tables = await _tableRepository.GetTablesAsync();
        var table = tables.FirstOrDefault(t => t.SessionToken == token && t.IsActive);

        if (table == null)
        {
            throw new Exception("Invalid or expired table token.");
        }

        var categories = await _categoryRepository.GetCategoriesAsync();
        
        // This is a simplified way to fetch products and options without eager loading explicitly in repo
        var products = await _productRepository.GetProductsAsync();

        var response = new TableMenuResponseDto
        {
            Table = new TableInfoDto
            {
                Id = table.TableId,
                Name = table.TableName,
                SessionToken = table.SessionToken
            },
            Categories = categories.Select(c => new MenuCategoryDto
            {
                Id = c.CategoryId,
                Name = c.Name,
                Products = products.Where(p => p.CategoryId == c.CategoryId).Select(p => new MenuItemDto
                {
                    Id = p.ProductId,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    IsAvailable = !p.IsDisabled && p.Stock_Quantity > 0,
                    Options = p.Options?.Select(o => new MenuOptionDto
                    {
                        Id = o.ProductOptionId,
                        Name = o.Name,
                        PriceAdjustment = o.PriceAdjustment
                    }).ToList() ?? new System.Collections.Generic.List<MenuOptionDto>()
                }).ToList()
            }).ToList()
        };

        return response;
    }
}
