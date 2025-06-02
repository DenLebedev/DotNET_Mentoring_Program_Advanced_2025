using System.Security.Claims;
using CatalogService.Application.DTOs;
using CatalogService.Application.Intefaces;
using CatalogService.Application.Services;
using CatalogService.GraphQL;
using CatalogService.GraphQL.Inputs;
using Moq;
using Xunit;

namespace CatalogService.Tests.GraphQL;

public class MutationTests
{
    [Fact]
    public async Task AddCategory_ReturnsCreatedCategory()
    {
        // Arrange
        var input = new CreateCategoryInput(
            "Books",
            "https://example.com/image.png",
            null
        );

        var expectedDto = new CategoryDto
        {
            Id = 1,
            Name = input.Name,
            ImageUrl = input.ImageUrl,
            ParentCategoryId = input.ParentCategoryId
        };

        var mockService = new Mock<ICategoryService>();
        mockService.Setup(s => s.AddAsync(It.IsAny<CreateCategoryDto>()))
                   .ReturnsAsync(expectedDto);

        var mutation = new Mutation();

        // Create a mock ClaimsPrincipal to satisfy the 'user' parameter
        var mockUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "TestUser") }));

        // Act
        var result = await mutation.AddCategory(input, mockService.Object, mockUser);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.Name, result.Name);
        Assert.Equal(expectedDto.ImageUrl, result.ImageUrl);
    }
}


