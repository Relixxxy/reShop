using AutoMapper;
using Catalog.Data.Entities;
using Catalog.Host.Models.Dtos;

namespace Catalog.Host.Mapper;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<ProductEntity, ProductDto>()
			.ForMember("PictureUrl", opt
				=> opt.MapFrom<ProductPictureResolver, string>(c => c.PictureFileName));
	}
}
