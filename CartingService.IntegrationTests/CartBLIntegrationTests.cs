using System.Net.Http.Json;
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
            var cartId = 1;
            var item = new Item
            {
                Id = 1,
                Name = "Test Item",
                ImageURL = "http://example.com/image.jpg",
                ImageAltText = "Image",
                Price = 10.0m,
                Quantity = 1
            };

            // Act
            var response = await client.PutAsJsonAsync($"/api/cartingservice/add-item/{cartId}", item);

            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"StatusCode: {response.StatusCode}");
            Console.WriteLine($"Response Body: {body}");

            // Assert
            response.EnsureSuccessStatusCode(); // should now pass
        }
    }
}