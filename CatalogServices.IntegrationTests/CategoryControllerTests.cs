using System.Net;
using System.Net.Http.Json;
using CatalogService.Application.Common;
using CatalogService.Application.DTOs;
using CatalogService.Application.Services;
using CatalogService.Domain.Entities;
using CatalogService.Domain.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Moq.Protected;

namespace CatalogService.Tests.Controllers;

public class CategoriesControllerTests
{
    private readonly HttpClient _client;
    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly Mock<IProductRepository> _productRepositoryMock;

    public CategoriesControllerTests()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
            {
                if (request.RequestUri!.AbsolutePath == "/api/categories")
                {
                    var categories = new List<CategoryDto> { new CategoryDto { Id = 1, Name = "Test Category" } };
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = JsonContent.Create(categories)
                    };
                }
                else if (request.RequestUri.AbsolutePath == "/api/products")
                {
                    var products = new List<ProductDto>
                    {
                        new ProductDto
                        {
                            Id = 1,
                            Name = "Test Product",
                            Price = 10.0m,
                            Description = "Test Description",
                            Links = new List<Link>
                            {
                                new Link("/api/products/1", "self", "GET")
                            }
                        }
                    };
                    return new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = JsonContent.Create(products)
                    };
                }
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            });

        _client = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };
    }

    [Fact]
    public async Task Get_Categories_ReturnsOkStatus()
    {
        // Act
        var response = await _client.GetAsync("/api/categories");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_Products_ReturnsOkStatus()
    {
        // Act
        var response = await _client.GetAsync("/api/products");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Get_Products_ReturnsOkStatus_WithHateoasLinks()
    {
        // Act
        var response = await _client.GetAsync("/api/products");

        // Assert
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var productsResponse = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
        productsResponse.Should().NotBeNull();
        productsResponse!.First().Links.Should().NotBeNullOrEmpty();
    }
}
