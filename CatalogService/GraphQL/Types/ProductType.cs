using CatalogService.Application.DTOs;
using HotChocolate.Types;

namespace CatalogService.GraphQL.Types;

public class ProductType : ObjectType<ProductDto>
{
    protected override void Configure(IObjectTypeDescriptor<ProductDto> descriptor)
    {
        descriptor.Description("Represents a product.");

        descriptor
            .Field(p => p.Id)
            .Type<NonNullType<IdType>>();

        descriptor
            .Field(p => p.Name)
            .Type<NonNullType<StringType>>();

        descriptor
            .Field(p => p.Description)
            .Type<StringType>();

        descriptor
            .Field(p => p.ImageUrl)
            .Type<StringType>();

        descriptor
            .Field(p => p.Price)
            .Type<FloatType>();

        descriptor
            .Field(p => p.Amount)
            .Type<IntType>();

        descriptor
            .Field(p => p.CategoryId)
            .Type<NonNullType<IdType>>();
    }
}
