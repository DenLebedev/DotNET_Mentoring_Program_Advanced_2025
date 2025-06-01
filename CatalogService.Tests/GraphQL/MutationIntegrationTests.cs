using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using CatalogService.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CatalogService.Tests.GraphQL;

public class MutationIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public MutationIntegrationTests(WebApplicationFactory<Program> factory)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");

        var appFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Test");
            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                configBuilder.AddJsonFile("appsettings.Test.json");
            });

            builder.ConfigureAppConfiguration((context, config) =>
            {
                context.HostingEnvironment.EnvironmentName = "Test";
            });

            builder.ConfigureServices(services =>
            {
                // Optionally log configuration or ensure the JwtBearer is loaded
            });
        });

        using var scope = appFactory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        _client = appFactory.CreateClient();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", TestTokenProvider.GetManagerToken());
    }

    [Fact]
    public async Task AddCategory_IntegrationTest()
    {
        Console.WriteLine("JWT: " + TestTokenProvider.GetManagerToken());

        // Arrange
        var mutation = @"
            mutation {
                addCategory(input: {
                    name: ""Test Category"",
                    imageUrl: ""https://example.com/image.png"",
                    parentCategoryId: null
                }) {
                    name
                    imageUrl
                }
            }";

        var requestContent = new StringContent(
            JsonSerializer.Serialize(new { query = mutation }),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await _client.PostAsync("/graphql", requestContent);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();

        // Parse the response as JSON
        var jsonResponse = JsonSerializer.Deserialize<JsonElement>(content);

        // Extract the data field from the response
        if (!jsonResponse.TryGetProperty("data", out var dataNode) || dataNode.ValueKind == JsonValueKind.Null)
        {
            if (jsonResponse.TryGetProperty("errors", out var errors))
            {
                Console.WriteLine("=== GRAPHQL ERRORS ===");
                Console.WriteLine(errors);
            }

            throw new Exception("Mutation failed or returned null. Possibly due to failed authorization or a server error.");
        }

        if (!dataNode.TryGetProperty("addCategory", out var resultNode) || resultNode.ValueKind == JsonValueKind.Null)
        {
            throw new Exception("Mutation did not return 'addCategory' or it was null.");
        }

        // Final assertions
        Assert.Equal("Test Category", resultNode.GetProperty("name").GetString());
        Assert.Equal("https://example.com/image.png", resultNode.GetProperty("imageUrl").GetString());

    }
}
