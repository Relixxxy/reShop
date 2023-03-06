using AutoMapper;
using Infrastructure.Models.Dtos;
using MVC.ViewModels;

namespace MVC.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<CatalogProductDto, ProductVM>();
        CreateMap<ProductVM, CatalogProductDto>();

        CreateMap<BasketProductDto, ProductVM>();
        CreateMap<ProductVM, BasketProductDto>();

        CreateMap<OrderDto, OrderVM>();
        CreateMap<OrderVM, OrderDto>();
    }
}
