using System.Net;
using Catalog.Host.Models.Requests;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Infrastructure.Models.Requests;
using Infrastructure.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowClientPolicy)]
    [Scope("catalog.products")]
    [Route(ComponentDefaults.DefaultRoute)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductsService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Add(CreateProductRequest request)
        {
            var result = await _productService.AddProductAsync(request.Name, request.Description, request.Price, request.AvailableStock, request.CatalogBrand, request.CatalogType, request.PictureFileName);

            return Ok(new AddItemResponse<int?>() { Id = result });
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(UpdateProductRequest request)
        {
            var result = await _productService.UpdateProductAsync(request.Id, request.Name, request.Description, request.Price, request.AvailableStock, request.CatalogBrand, request.CatalogType, request.PictureFileName);

            _logger.LogInformation($"Update product result -> {result}");

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(IdRequest request)
        {
            var result = await _productService.DeleteProductAsync(request.Id);

            _logger.LogInformation($"Delete product result -> {result}");

            return Ok();
        }
    }
}
