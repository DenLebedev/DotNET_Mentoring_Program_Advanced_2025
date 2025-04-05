﻿namespace CatalogService.Application.DTOs;

public class UpdateCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? ImageUrl { get; set; }
    public int? ParentCategoryId { get; set; }
}