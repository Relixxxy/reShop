using AutoMapper;
using Order.Data.Entities;
using Infrastructure.Models.Dtos;

namespace Order.Host.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<OrderEntity, OrderDto>();

        CreateMap<ProductEntity, OrderProductDto>();
        CreateMap<OrderProductDto, ProductEntity>();
    }
}
