using AutoMapper;
using Catalog.Data.Entities;
using Catalog.Host.Models.Dtos;

namespace Catalog.Host.Mapper;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<ProductEntity, ProductDto>()
			.ForMember(pd => pd.Amount, opt => opt.MapFrom(pe => pe.AvailableStock))
			.ForMember("PictureUrl", opt
				=> opt.MapFrom<ProductPictureResolver, string>(c => c.PictureFileName));
	}
}
