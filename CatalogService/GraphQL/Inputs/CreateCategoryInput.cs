namespace CatalogService.GraphQL.Inputs;

public record CreateCategoryInput(
    string Name,
    string? ImageUrl,
    int? ParentCategoryId
);
