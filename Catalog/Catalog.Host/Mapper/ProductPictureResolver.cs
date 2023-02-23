using AutoMapper;
using Catalog.Data.Entities;
using Catalog.Host.Configurations;
using Catalog.Host.Models.Dtos;
using Microsoft.Extensions.Options;

public class ProductPictureResolver : IMemberValueResolver<ProductEntity, ProductDto, string, object>
{
    private readonly CatalogConfig _config;

    public ProductPictureResolver(IOptionsSnapshot<CatalogConfig> config)
    {
        _config = config.Value;
    }

    public object Resolve(ProductEntity source, ProductDto destination, string sourceMember, object destMember, ResolutionContext context)
    {
        return $"{_config.CdnHost}/{_config.ImgUrl}/{sourceMember}";
    }
}