namespace CatalogService.GraphQL.Inputs;

public record UpdateCategoryInput(
    string Name,
    string? ImageUrl,
    int? ParentCategoryId
);
