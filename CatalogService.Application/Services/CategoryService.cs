using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application.Services;

public class CategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task AddAsync(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name) || category.Name.Length > 50)
            throw new ArgumentException("Name is required and must be less than or equal to 50 characters.");

        await _repository.AddAsync(category);
    }

    public async Task UpdateAsync(Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name) || category.Name.Length > 50)
            throw new ArgumentException("Name is required and must be less than or equal to 50 characters.");

        await _repository.UpdateAsync(category);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
