using AutoMapper;
using CatalogService.Application.DTOs;
using CatalogService.Application.Services;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace CatalogService.Tests.Services;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper; // Add this field
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _mockRepo = new Mock<ICategoryRepository>();
        _mockMapper = new Mock<IMapper>(); // Initialize the mock mapper
        _service = new CategoryService(_mockRepo.Object, _mockMapper.Object); // Pass the mock mapper to the service
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

        _mockRepo.Verify(r => r.AddAsync(It.Is<Category>(c => c.Name == "Valid Name")), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_IfNotFound()
    {
        _mockRepo.Setup(r => r.GetByIdAsync(42)).ReturnsAsync((Category?)null);

        var result = await _service.GetByIdAsync(42);

        result.Should().BeNull();
    }
}
