using System.Collections.Generic;

namespace SmartQRCoffee.Services.DTOs;

public class TableInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? SessionToken { get; set; }
}

public class MenuOptionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal PriceAdjustment { get; set; }
}

public class MenuItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsAvailable { get; set; }
    public List<MenuOptionDto> Options { get; set; } = new List<MenuOptionDto>();
}

public class MenuCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<MenuItemDto> Products { get; set; } = new List<MenuItemDto>();
}

public class TableMenuResponseDto
{
    public TableInfoDto Table { get; set; } = null!;
    public List<MenuCategoryDto> Categories { get; set; } = new List<MenuCategoryDto>();
}
