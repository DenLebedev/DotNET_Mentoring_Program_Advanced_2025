using CatalogService.Application.DTOs;

namespace CatalogService.Application.Intefaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync(int? categoryId, int page, int pageSize);
        Task<ProductDto?> GetByIdAsync(int id);
        Task<ProductDto> AddAsync(CreateProductDto productDto);
        Task<ProductDto?> UpdateAsync(int id, UpdateProductDto productDto);
        Task DeleteAsync(int id);
        Task<ILookup<int, ProductDto>> GetProductsByCategoryIdsAsync(IEnumerable<int> categoryIds, CancellationToken cancellationToken);

    }
}
