﻿using AutoMapper;
using CatalogService.Application.DTOs;
using CatalogService.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CatalogService.Application.Mappings;

public class CatalogMappingProfile : Profile
{
    public CatalogMappingProfile()
    {
        // Category
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Category, UpdateCategoryDto>();

        // Product
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Product, UpdateProductDto>();
    }
}
