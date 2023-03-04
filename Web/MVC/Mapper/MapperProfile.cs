using AutoMapper;
using Infrastructure.Models.Dtos;
using MVC.ViewModels;

namespace MVC.Mapper;

public class MapperProfile : Profile
{
	public MapperProfile()
	{
		CreateMap<CatalogProductDto, ProductVM>();
	}
}
