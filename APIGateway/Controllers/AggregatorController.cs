using APIGateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers;

[ApiController]
[Route("api/aggregated/products/{id}")]
public class AggregatorController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public AggregatorController(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("CatalogService");
    }

    [HttpGet]
    public async Task<IActionResult> GetFullProductDetails(int id)
    {
        // Test hint: This is a test hint to check if the code is working correctly.
        Console.WriteLine($"AggregatorController hit for ID {id}");

        var productTask = _httpClient.GetFromJsonAsync<ProductDto>($"/api/products/{id}");
        var propsTask = _httpClient.GetFromJsonAsync<Dictionary<string, string>>($"/api/products/{id}/properties");

        await Task.WhenAll(productTask, propsTask);

        var product = await productTask;
        var props = await propsTask;

        if (product == null)
            return NotFound();

        var result = new
        {
            product.Id,
            product.Name,
            product.Price,
            product.Description,
            Properties = props
        };

        return Ok(result);
    }
}
