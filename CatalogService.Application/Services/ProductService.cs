using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application.Services;

public class ProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name) || product.Name.Length > 50)
            throw new ArgumentException("Name is required and must be less than or equal to 50 characters.");
        if (product.Amount < 0)
            throw new ArgumentException("Amount must be positive.");
        if (product.Price < 0)
            throw new ArgumentException("Price must be positive.");

        await _repository.AddAsync(product);
    }

    public async Task UpdateAsync(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name) || product.Name.Length > 50)
            throw new ArgumentException("Name is required and must be less than or equal to 50 characters.");
        if (product.Amount < 0)
            throw new ArgumentException("Amount must be positive.");
        if (product.Price < 0)
            throw new ArgumentException("Price must be positive.");

        await _repository.UpdateAsync(product);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
