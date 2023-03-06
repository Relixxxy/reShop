using AutoMapper;
using Catalog.Data;
using Catalog.Data.Repositories.Interfaces;
using Infrastructure.Models.Enums;
using Catalog.Host.Models.Responses;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Models.Dtos;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;

namespace Catalog.Host.Services;

public class CatalogService : BaseDataService<ApplicationDbContext>, ICatalogService
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<CatalogService> _logger;
    private readonly IMapper _mapper;

    public CatalogService(
        IProductsRepository productRepository,
        IMapper mapper,
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<CatalogService> catalogLog,
        ILogger<BaseDataService<ApplicationDbContext>> baseServiceLog)
        : base(dbContextWrapper, baseServiceLog)
    {
        _productsRepository = productRepository;
        _logger = catalogLog;
        _mapper = mapper;
    }

    public async Task<PaginatedItemsResponse<CatalogProductDto>> GetProductsAsync(int pageIndex, int pageSize, Dictionary<ProductTypeFilter, string>? filters)
    {
        return await ExecuteSafeAsync(async () =>
        {
            string? brandFilter = null;
            string? typeFilter = null;

            if (filters is not null)
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

            var result = await _productsRepository.GetProductsByPageAsync(pageIndex, pageSize, brandFilter, typeFilter);

            if (result is null)
            {
                _logger.LogWarning($"Products not found");
                return null!;
            }

            _logger.LogInformation($"Found {result.TotalCount} products");

            return new PaginatedItemsResponse<CatalogProductDto>()
            {
                Count = result.TotalCount,
                Data = result.Data.Select(_mapper.Map<CatalogProductDto>).ToList(),
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        });
    }

    public async Task<CatalogProductDto> GetProductAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var entity = await _productsRepository.GetProductByIdAsync(id);
            var product = _mapper.Map<CatalogProductDto>(entity);

            _logger.LogInformation($"ProductEntity with id ({entity.Id}) successfully got from repo and mapped to dto");

            return product;
        });
    }

    public async Task<IEnumerable<string>> GetBrandsAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var brands = await _productsRepository.GetBrandsAsync();

            _logger.LogInformation($"Found {brands.Count()} brands");

            return brands;
        });
    }

    public async Task<IEnumerable<string>> GetTypesAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var types = await _productsRepository.GetTypesAsync();

            _logger.LogInformation($"Found {types.Count()} types");

            return types;
        });
    }
}
