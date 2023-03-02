using AutoMapper;
using Order.Data.Entities;
using Order.Host.Models.Dtos;

namespace Order.Host.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderEntity, OrderDto>();
            CreateMap<ProductEntity, ProductDto>();
        }
    }
}
