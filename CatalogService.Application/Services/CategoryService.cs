using AutoMapper;
using CatalogService.Application.DTOs;
using CatalogService.Application.Intefaces;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);
        return _mapper.Map<CategoryDto?>(category);
    }

    public async Task<CategoryDto> AddAsync(CreateCategoryDto categoryDto)
    {
        if (string.IsNullOrWhiteSpace(categoryDto.Name) || categoryDto.Name.Length > 50)
            throw new ArgumentException("Name is required and must be less than or equal to 50 characters.");

        var category = _mapper.Map<Category>(categoryDto);
        await _repository.AddAsync(category);
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto categoryDto)
    {
        var category = await _repository.GetByIdAsync(id);
        if (category == null)
        {
            return null;
        }
        else
        {
            if (string.IsNullOrWhiteSpace(categoryDto.Name) || categoryDto.Name.Length > 50)
                throw new ArgumentException("Name is required and must be less than or equal to 50 characters.");
        }

        _mapper.Map(categoryDto, category);
        await _repository.UpdateAsync(category);
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
