using System.Security.Claims;
using CatalogService.Application.DTOs;
using CatalogService.Application.Intefaces;
using CatalogService.Application.Services;
using CatalogService.GraphQL.Inputs;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;

namespace CatalogService.GraphQL;

public class Mutation
{
    [Authorize(Roles = new[] { "Manager" })]
    public async Task<CategoryDto> AddCategory(
        CreateCategoryInput input,
        [Service] ICategoryService service,
        ClaimsPrincipal user)
    {
        var dto = new CreateCategoryDto
        {
            Name = input.Name,
            ImageUrl = input.ImageUrl,
            ParentCategoryId = input.ParentCategoryId
        };

        return await service.AddAsync(dto);
    }

    [Authorize(Roles = new[] { "Manager" })]
    public async Task<CategoryDto?> UpdateCategory(
        int id,
        UpdateCategoryInput input,
        [Service] CategoryService service)
    {
        var dto = new UpdateCategoryDto
        {
            Name = input.Name,
            ImageUrl = input.ImageUrl,
            ParentCategoryId = input.ParentCategoryId
        };

        return await service.UpdateAsync(id, dto);
    }

    [Authorize(Roles = new[] { "Manager" })]
    public async Task<bool> DeleteCategory(
        int id,
        [Service] CategoryService service)
    {
        await service.DeleteCategoryWithProductsAsync(id);
        return true;
    }

    [Authorize(Roles = new[] { "Manager" })]
    public async Task<ProductDto> AddProduct(
        CreateProductInput input,
        [Service] ProductService service)
    {
        var dto = new CreateProductDto
        {
            Name = input.Name,
            Description = input.Description,
            ImageUrl = input.ImageUrl,
            Price = input.Price,
            Amount = input.Amount,
            CategoryId = input.CategoryId
        };

        return await service.AddAsync(dto);
    }

    [Authorize(Roles = new[] { "Manager" })]
    public async Task<ProductDto?> UpdateProduct(
        int id,
        UpdateProductInput input,
        [Service] ProductService service)
    {
        var dto = new UpdateProductDto
        {
            Name = input.Name,
            Description = input.Description,
            ImageUrl = input.ImageUrl,
            Price = input.Price,
            Amount = input.Amount,
            CategoryId = input.CategoryId
        };

        return await service.UpdateAsync(id, dto);
    }

    [Authorize(Roles = new[] { "Manager" })]
    public async Task<bool> DeleteProduct(
        int id,
        [Service] ProductService service)
    {
        await service.DeleteAsync(id);
        return true;
    }
}
