using CatalogService.Domain.Entities;

namespace CatalogService.Domain.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync(int? categoryId, int page, int pageSize);
    Task<Product?> GetByIdAsync(int id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
    Task<IEnumerable<Product>> GetByCategoryIdsAsync(IEnumerable<int> categoryIds, CancellationToken cancellationToken);
}

