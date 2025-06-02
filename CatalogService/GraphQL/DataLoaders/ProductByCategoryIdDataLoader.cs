using System.Collections.Concurrent;
using CatalogService.Application.DTOs;
using CatalogService.Application.Intefaces;
using GreenDonut;

namespace CatalogService.GraphQL.DataLoaders;

public class ProductByCategoryIdDataLoader : BatchDataLoader<int, IEnumerable<ProductDto>>
{
    private readonly IProductService _productService;

    public ProductByCategoryIdDataLoader(
        IProductService productService,
        IBatchScheduler batchScheduler,
        DataLoaderOptions? options = null)
        : base(batchScheduler, options)
    {
        _productService = productService;
    }

    protected override async Task<IReadOnlyDictionary<int, IEnumerable<ProductDto>>> LoadBatchAsync(
    IReadOnlyList<int> categoryIds,
    CancellationToken cancellationToken)
    {
        var result = await _productService.GetProductsByCategoryIdsAsync(categoryIds, cancellationToken);

        return result.ToDictionary(g => g.Key, g => g.AsEnumerable());
    }
}
