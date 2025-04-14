using System.Net.Http.Json;
using CartingService.DTOs;
using CartingService.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace CartingService.IntegrationTests
{
    public class CartBLIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public CartBLIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AddItem_ShouldAddItemToCart()
        {
            // Arrange
            var client = _factory.CreateClient();
            var cartKey = "testCart";
            var itemDto = new ItemDto
            {
                Id = 1,
                Name = "Test Item",
                ImageURL = "http://example.com/image.jpg",
                ImageAltText = "Image",
                Price = 10.0m,
                Quantity = 1
            };

            // Act
            var response = await client.PostAsJsonAsync($"/api/v1/cart/{cartKey}/items", itemDto);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteItem_ShouldRemoveItemFromCart()
        {
            // Arrange
            var client = _factory.CreateClient();
            var cartKey = "testCart";
            var itemDto = new ItemDto
            {
                Id = 1,
                Name = "Test Item",
                ImageURL = "http://example.com/image.jpg",
                ImageAltText = "Image",
                Price = 10.0m,
                Quantity = 1
            };

            // Add item to cart first
            await client.PostAsJsonAsync($"/api/v1/cart/{cartKey}/items", itemDto);

            // Act
            var response = await client.DeleteAsync($"/api/v1/cart/{cartKey}/items/{itemDto.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
