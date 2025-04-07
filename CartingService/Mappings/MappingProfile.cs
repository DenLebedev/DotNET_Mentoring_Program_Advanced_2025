using AutoMapper;
using CartingService.DTOs;
using CartingService.Entities;

namespace CartingService.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Define your mappings here
            CreateMap<Item, ItemDto>().ReverseMap();
            CreateMap<Cart, CartDto>().ReverseMap();
        }
    }
}
