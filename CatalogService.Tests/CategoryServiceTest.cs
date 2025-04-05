using CatalogService.Application.Services;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace CatalogService.Tests.Services;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _mockRepo;
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _mockRepo = new Mock<ICategoryRepository>();
        _service = new CategoryService(_mockRepo.Object);
    }

    [Fact]
    public async Task AddAsync_ThrowsException_WhenNameIsEmpty()
    {
        var category = new Category { Name = "" };

        Func<Task> act = async () => await _service.AddAsync(category);

        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Name is required and must be less than or equal to 50 characters.");
    }

    [Fact]
    public async Task AddAsync_CallsRepository_WhenValid()
    {
        var category = new Category { Name = "Valid Name" };

        await _service.AddAsync(category);

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