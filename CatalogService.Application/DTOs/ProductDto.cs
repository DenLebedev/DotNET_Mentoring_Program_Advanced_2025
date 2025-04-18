﻿using CatalogService.Application.Common;

namespace CatalogService.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal Price { get; set; }
    public int Amount { get; set; }
    public int CategoryId { get; set; }
    public List<Link> Links { get; set; } = new List<Link>();
}
