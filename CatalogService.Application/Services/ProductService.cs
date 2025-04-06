using AutoMapper;
using CatalogService.Application.DTOs;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;

namespace CatalogService.Application.Services;

public class ProductService
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync(int? categoryId, int page, int pageSize)
    {
        var products = await _repository.GetProductsAsync(categoryId, page, pageSize);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return _mapper.Map<ProductDto?>(product);
    }

    public async Task<ProductDto> AddAsync(CreateProductDto productDto)
    {
        if (string.IsNullOrWhiteSpace(productDto.Name) || productDto.Name.Length > 50)
            throw new ArgumentException("Name is required and must be less than or equal to 50 characters.");
        if (productDto.Amount < 0)
            throw new ArgumentException("Amount must be positive.");
        if (productDto.Price < 0)
            throw new ArgumentException("Price must be positive.");

        var product = _mapper.Map<Product>(productDto);
        await _repository.AddAsync(product);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto productDto)
    {
        var product = await _repository.GetByIdAsync(id);
        if (product == null)
        {
            return null;
        }
        else
        {
            if (string.IsNullOrWhiteSpace(productDto.Name) || productDto.Name.Length > 50)
                throw new ArgumentException("Name is required and must be less than or equal to 50 characters.");
            if (productDto.Amount < 0)
                throw new ArgumentException("Amount must be positive.");
            if (productDto.Price < 0)
                throw new ArgumentException("Price must be positive.");
        }

        _mapper.Map(productDto, product);
        await _repository.UpdateAsync(product);
        return _mapper.Map<ProductDto>(product);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }
}
