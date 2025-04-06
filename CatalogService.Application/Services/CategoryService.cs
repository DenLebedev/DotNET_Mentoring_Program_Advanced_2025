using AutoMapper;
using CatalogService.Application.DTOs;
using CatalogService.Application.Intefaces;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository repository, IProductRepository productRepository, IMapper mapper)
    {
        _categoryRepository = repository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return _mapper.Map<CategoryDto?>(category);
    }

    public async Task<CategoryDto> AddAsync(CreateCategoryDto categoryDto)
    {
        if (string.IsNullOrWhiteSpace(categoryDto.Name) || categoryDto.Name.Length > 50)
            throw new ArgumentException("Name is required and must be less than or equal to 50 characters.");

        var category = _mapper.Map<Category>(categoryDto);
        await _categoryRepository.AddAsync(category);
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto categoryDto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
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
        await _categoryRepository.UpdateAsync(category);
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task DeleteAsync(int id)
    {
        await _categoryRepository.DeleteAsync(id);
    }

    public async Task DeleteCategoryWithProductsAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            throw new ArgumentException("Category not found.");
        }

        // Delete related products
        var products = await _productRepository.GetProductsAsync(id, 1, int.MaxValue);
        foreach (var product in products)
        {
            await _productRepository.DeleteAsync(product.Id);
        }

        // Delete the category
        await _categoryRepository.DeleteAsync(id);
    }
}
