using AutoMapper;
using Catalog.Data;
using Catalog.Data.Repositories.Interfaces;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Services.Interfaces;
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
        return ExecuteSafeAsync(() => _productsRepository.AddProductAsync(name, desc, availableStock, availableStock, pictureName, type, brand));
    }

    public Task<bool> DeleteProductAsync(int id)
    {
        return ExecuteSafeAsync(() => _productsRepository.DeleteProductAsync(id));
    }

    public async Task<ProductDto> GetProductAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var entity = await _productsRepository.GetProductByIdAsync(id);
            var product = _mapper.Map<ProductDto>(entity);

            _logger.LogInformation("ProductEntity successfully got from repo and mapped to dto");

            return product;
        });
    }

    public Task<bool> UpdateProductAsync(int id, string name, string desc, decimal price, int availableStock, string pictureName, string type, string brand)
    {
        return ExecuteSafeAsync(() => _productsRepository.UpdateProductAsync(id, name, desc, availableStock, availableStock, pictureName, type, brand));
    }
}
