using System.Net.Http.Json;
using CatalogService.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;

namespace CatalogService.Tests.Controllers;

public class CategoriesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CategoriesControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Categories_ReturnsOkStatus()
    {
        var response = await _client.GetAsync("/api/categories");

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Post_ThenGet_Category_ReturnsCreatedItem()
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

        Dispose();
    }

    [Fact]
    public async Task Get_Products_ReturnsOkStatus()
    {
        var response = await _client.GetAsync("/api/products");

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_Products_ReturnsOkStatus_WithHateoasLinks()
    {
        var response = await _client.GetAsync("/api/products");

        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
        products.Should().NotBeNull();
        products!.First().Links.Should().NotBeNullOrEmpty();
    }

    public void Dispose()
    {
        CleanupCategoryTestData().GetAwaiter().GetResult();
    }

    private async Task CleanupCategoryTestData()
    {
        var response = await _client.GetAsync("/api/categories");
        var categories = await response.Content.ReadFromJsonAsync<IEnumerable<CategoryDto>>();

        foreach (var category in categories.Where(c => c.Name == "Test Category"))
        {
            await _client.DeleteAsync($"/api/categories/{category.Id}");
        }
    }
}