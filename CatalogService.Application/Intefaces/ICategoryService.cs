using CatalogService.Application.DTOs;

namespace CatalogService.Application.Intefaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<CategoryDto> AddAsync(CreateCategoryDto categoryDto);
        Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto categoryDto);
        Task DeleteAsync(int id);
        Task DeleteCategoryWithProductsAsync(int id);
    }
}
