using CatalogService.Application.Common;

namespace CatalogService.Application.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public int? ParentCategoryId { get; set; }
    public List<Link> Links { get; set; } = new List<Link>();
}

