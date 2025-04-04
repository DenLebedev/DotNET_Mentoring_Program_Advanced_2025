using System.Net.Http.Json;
using CatalogService.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace CatalogService.Tests.Controllers;

public class CategoriesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CategoriesControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsOkStatus()
    {
        var response = await _client.GetAsync("/api/categories");

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_ThenGet_ReturnsCreatedItem()
    {
        // Arrange
        var newCategory = new CreateCategoryDto { Name = "Test Category" };

        // Act
        var postResponse = await _client.PostAsJsonAsync("/api/categories", newCategory);
        postResponse.EnsureSuccessStatusCode();

        var created = await postResponse.Content.ReadFromJsonAsync<CategoryDto>();

        // Assert
        created.Should().NotBeNull();
        created!.Name.Should().Be("Test Category");

        var getResponse = await _client.GetAsync($"/api/categories/{created.Id}");
        getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}
