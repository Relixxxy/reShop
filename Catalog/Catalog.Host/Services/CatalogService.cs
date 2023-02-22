using AutoMapper;
using Catalog.Data;
using Catalog.Data.Repositories.Interfaces;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Responses;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogService : BaseDataService<ApplicationDbContext>, ICatalogService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CatalogService> _logger;
    private readonly IMapper _mapper;

    public CatalogService(
        IProductRepository productRepository,
        IMapper mapper,
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogService> catalogLog,
        ILogger<BaseDataService<ApplicationDbContext>> baseServiceLog)
        : base(dbContextWrapper, baseServiceLog)
    {
        _productRepository = productRepository;
        _logger = catalogLog;
        _mapper = mapper;
    }

    public async Task<PaginatedItemsResponse<ProductDto>> GetProductsAsync(int pageIndex, int pageSize, Dictionary<ProductTypeFilter, string>? filters)
    {
        return await ExecuteSafeAsync(async () =>
        {
            string? brandFilter = null;
            string? typeFilter = null;

            if (filters != null)
            {
                if (filters.TryGetValue(ProductTypeFilter.Brand, out var brand))
                {
                    brandFilter = brand;
                }

                if (filters.TryGetValue(ProductTypeFilter.Type, out var type))
                {
                    typeFilter = type;
                }
            }

            var result = await _productRepository.GetProductsByPageAsync(pageIndex, pageSize, brandFilter, typeFilter);

            if (result == null)
            {
                return null!;
            }

            return new PaginatedItemsResponse<ProductDto>()
            {
                Count = result.Count(),
                Data = result.Select(_mapper.Map<ProductDto>).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        });
    }
}
