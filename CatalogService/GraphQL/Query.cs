using CatalogService.Application.DTOs;
using CatalogService.Application.Services;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;

namespace CatalogService.GraphQL;

public class Query
{
    [Authorize]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<CategoryDto>> GetCategories(
        [Service] CategoryService categoryService)
    {
        return await categoryService.GetAllAsync();
    }

    [Authorize]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<ProductDto>> GetProducts(
    [Service] ProductService productService,
    int? categoryId,
    int page = 1,
    int pageSize = 100)
    {
        return await productService.GetProductsAsync(categoryId, page, pageSize);
    }
}
