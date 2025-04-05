namespace CatalogService.Application.DTOs;

public class CreateCategoryDto
{
    public string Name { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public int? ParentCategoryId { get; set; }
}
