namespace CatalogService.Application.DTOs;

public class CreateProductDto
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public int CategoryId { get; set; }
}
