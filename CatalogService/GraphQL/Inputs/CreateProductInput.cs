namespace CatalogService.GraphQL.Inputs;

public record CreateProductInput(
    string Name,
    string? Description,
    string? ImageUrl,
    decimal Price,
    int Amount,
    int CategoryId
);
