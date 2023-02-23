using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Responses;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsResponse<ProductDto>> GetProductsAsync(int pageIndex, int pageSize, Dictionary<ProductTypeFilter, string>? filters);
}
