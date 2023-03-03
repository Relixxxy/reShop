using AutoMapper;
using Catalog.Data;
using Catalog.Data.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;
using Infrastructure.Models.Dtos;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;

namespace Catalog.Host.Services;

public class ProductsService : BaseDataService<ApplicationDbContext>, IProductsService
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<ProductsService> _logger;
    private readonly IMapper _mapper;

    public ProductsService(
        IProductsRepository productRepository,
        IMapper mapper,
        IDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<ProductsService> catalogLog,
        ILogger<BaseDataService<ApplicationDbContext>> baseServiceLog)
        : base(dbContextWrapper, baseServiceLog)
    {
        _productsRepository = productRepository;
        _logger = catalogLog;
        _mapper = mapper;
    }

    public Task<int?> AddProductAsync(string name, string desc, decimal price, int availableStock, string pictureName, string type, string brand)
    {
        return ExecuteSafeAsync(async () =>
        {
            var result = await _productsRepository.AddProductAsync(name, desc, availableStock, availableStock, pictureName, type, brand);
            _logger.LogInformation($"Product with id ({result}) has added");
            return result;
        });
    }

    public Task<bool> DeleteProductAsync(int id)
    {
        return ExecuteSafeAsync(async () =>
        {
            var result = await _productsRepository.DeleteProductAsync(id);
            _logger.LogInformation($"Product with id ({result}) has deleted");
            return result;
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

    public Task<bool> UpdateProductAsync(int id, string name, string desc, decimal price, int availableStock, string pictureName, string type, string brand)
    {
        return ExecuteSafeAsync(async () =>
        {
            var result = await _productsRepository.UpdateProductAsync(id, name, desc, availableStock, availableStock, pictureName, type, brand);
            _logger.LogInformation($"Product with id ({result}) has updated");
            return result;
        });
    }
}
