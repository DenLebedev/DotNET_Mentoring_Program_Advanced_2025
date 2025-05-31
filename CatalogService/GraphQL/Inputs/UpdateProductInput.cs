namespace CatalogService.GraphQL.Inputs;

public record UpdateProductInput(
    string Name,
    string? Description,
    string? ImageUrl,
    decimal Price,
    int Amount,
    int CategoryId
);
