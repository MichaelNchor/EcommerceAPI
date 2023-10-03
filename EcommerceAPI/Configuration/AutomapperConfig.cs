using AutoMapper;
using EcommerceAPI.DTO.Cart;
using EcommerceAPI.DTO.Product;
using EcommerceAPI.Models;

namespace EcommerceAPI.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Product, ProductAddDTO>().ReverseMap();
            CreateMap<Product, ProductDeleteDTO>().ReverseMap();
            CreateMap<Product, ProductGetDTO>().ReverseMap();
            CreateMap<Product, ProductPutDTO>().ReverseMap();
            CreateMap<Cart, CartGetDTO>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Product));
            CreateMap<Cart, CartAddDTO>().ReverseMap();
        }
    }
}
