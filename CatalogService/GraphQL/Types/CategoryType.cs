using CatalogService.Application.DTOs;
using CatalogService.GraphQL.DataLoaders;
using HotChocolate.Types;

namespace CatalogService.GraphQL.Types;

public class CategoryType : ObjectType<CategoryDto>
{
    protected override void Configure(IObjectTypeDescriptor<CategoryDto> descriptor)
    {
        descriptor.Description("Represents a category of products.");

        descriptor
            .Field(c => c.Id)
            .Type<NonNullType<IdType>>();

        descriptor
            .Field(c => c.Name)
            .Type<NonNullType<StringType>>();

        descriptor
            .Field(c => c.ImageUrl)
            .Type<StringType>();

        descriptor
            .Field(c => c.ParentCategoryId)
            .Type<IdType>();
        descriptor
            .Field("products")
            .ResolveWith<CategoryResolvers>(r => r.GetProductsAsync(default!, default!, default))
            .Type<ListType<NonNullType<ObjectType<ProductDto>>>>()
            .Name("products");
    }

    private class CategoryResolvers
    {
        public async Task<IEnumerable<ProductDto>> GetProductsAsync(
            [Parent] CategoryDto category,
            ProductByCategoryIdDataLoader dataLoader,
            CancellationToken cancellationToken)
        {
            return await dataLoader.LoadAsync(category.Id, cancellationToken);
        }
    }
}
