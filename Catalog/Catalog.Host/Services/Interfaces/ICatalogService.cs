using Infrastructure.Models.Enums;
using Catalog.Host.Models.Responses;
using Infrastructure.Models.Dtos;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<CatalogProductDto>> GetProductsAsync(int pageIndex, int pageSize, Dictionary<ProductTypeFilter, string>? filters);
    Task<CatalogProductDto> GetProductAsync(int id);
    Task<IEnumerable<string>> GetBrandsAsync();
    Task<IEnumerable<string>> GetTypesAsync();
}
