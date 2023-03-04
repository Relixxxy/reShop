using AutoMapper;
using Catalog.Data.Entities;
using Infrastructure.Models.Dtos;

namespace Catalog.Host.Mapper;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<ProductEntity, CatalogProductDto>()
			.ForMember(pd => pd.Amount, opt => opt.MapFrom(pe => pe.AvailableStock))
			.ForMember("PictureUrl", opt
				=> opt.MapFrom<ProductPictureResolver, string>(c => c.PictureFileName));
	}
}
