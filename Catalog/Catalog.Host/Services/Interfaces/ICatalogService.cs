using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Responses;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<ProductDto>> GetProductsAsync(int pageIndex, int pageSize, Dictionary<ProductTypeFilter, string>? filters);
    Task<ProductDto> GetProductAsync(int id);
    Task<IEnumerable<string>> GetBrandsAsync();
    Task<IEnumerable<string>> GetTypesAsync();
}
