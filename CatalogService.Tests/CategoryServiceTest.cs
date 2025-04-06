using AutoMapper;
using CatalogService.Application.DTOs;
using CatalogService.Application.Intefaces;
using CatalogService.Application.Services;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace CatalogService.Tests.Services;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepo;
    private readonly Mock<IProductRepository> _mockProductRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _mockCategoryRepo = new Mock<ICategoryRepository>();
        _mockProductRepo = new Mock<IProductRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new CategoryService(_mockCategoryRepo.Object, _mockProductRepo.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task AddAsync_ThrowsException_WhenNameIsEmpty()
    {
        var createCategoryDto = new CreateCategoryDto { Name = "" };
        Category? category = new Category { Name = "" };

        _mockMapper.Setup(m => m.Map<Category>(It.IsAny<object>())).Returns(() => category);

        Func<Task> act = async () => await _service.AddAsync(createCategoryDto);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Name is required and must be less than or equal to 50 characters.");
    }

    [Fact]
    public async Task AddAsync_CallsRepository_WhenValid()
    {
        var createCategoryDto = new CreateCategoryDto { Name = "Valid Name" };
        var category = new Category { Name = "Valid Name" };

        _mockMapper.Setup(m => m.Map<Category>(It.IsAny<object>())).Returns(() => category);

        await _service.AddAsync(createCategoryDto);

        _mockCategoryRepo.Verify(r => r.AddAsync(It.Is<Category>(c => c.Name == "Valid Name")), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_IfNotFound()
    {
        _mockCategoryRepo.Setup(r => r.GetByIdAsync(42)).ReturnsAsync((Category?)null);

        var result = await _service.GetByIdAsync(42);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteCategoryWithProductsAsync_ShouldDeleteCategoryAndProducts()
    {
        // Arrange
        var categoryId = 1;
        var category = new Category { Id = categoryId };
        var products = new List<Product>
        {
            new Product { Id = 1, CategoryId = categoryId },
            new Product { Id = 2, CategoryId = categoryId }
        };

        _mockCategoryRepo.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
        _mockProductRepo.Setup(repo => repo.GetProductsAsync(categoryId, 1, int.MaxValue)).ReturnsAsync(products);

        // Act
        await _service.DeleteCategoryWithProductsAsync(categoryId);

        // Assert
        _mockProductRepo.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Exactly(products.Count));
        _mockCategoryRepo.Verify(repo => repo.DeleteAsync(categoryId), Times.Once);
    }
}
