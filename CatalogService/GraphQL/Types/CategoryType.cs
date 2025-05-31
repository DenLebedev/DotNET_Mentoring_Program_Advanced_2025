using CatalogService.Application.DTOs;
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
    }
}
