using MVC.Dtos;
using MVC.Models.Enums;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services;

public class CatalogService : ICatalogService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(IHttpClientService httpClient, ILogger<CatalogService> logger, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task<ProductsCatalog> GetCatalogItems(int page, int take, string? brand, string? type)
    {
        var filters = new Dictionary<CatalogTypeFilter, string>();

        if (brand != null && brand != "all")
        {
            filters.Add(CatalogTypeFilter.Brand, brand);
        }

        if (type != null && type != "all")
        {
            filters.Add(CatalogTypeFilter.Type, type);
        }

        var result = await _httpClient.SendAsync<ProductsCatalog, PaginatedItemsRequest<CatalogTypeFilter>>(
           $"{_settings.Value.CatalogUrl}/products",
           HttpMethod.Post,
           new PaginatedItemsRequest<CatalogTypeFilter>()
           {
               PageIndex = page,
               PageSize = take,
               Filters = filters
           });

        _logger.LogInformation($"Received {result.Count} products from catalog");

        return result!;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
        var list = new List<SelectListItem>();

        var result = await _httpClient.SendAsync<IEnumerable<string>, object>(
            $"{_settings.Value.CatalogUrl}/brands",
            HttpMethod.Post,
            new { });

        _logger.LogInformation($"Received {result.Count()} brands from catalog");

        foreach (var brand in result)
        {
            list.Add(new SelectListItem() { Value = brand, Text = brand });
        }

        return list;
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        var list = new List<SelectListItem>();

        var result = await _httpClient.SendAsync<IEnumerable<string>, object>(
            $"{_settings.Value.CatalogUrl}/types",
            HttpMethod.Post,
            new { });

        _logger.LogInformation($"Received {result.Count()} types from catalog");

        foreach (var type in result)
        {
            list.Add(new SelectListItem() { Value = type, Text = type });
        }

        return list;
    }
}
