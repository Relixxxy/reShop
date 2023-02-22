using System.Net;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers
{
    [ApiController]
    [Route(ComponentDefaults.DefaultRoute)]
    public class CatalogBffController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly ILogger<CatalogBffController> _logger;

        public CatalogBffController(ICatalogService catalogService, ILogger<CatalogBffController> logger)
        {
            _catalogService = catalogService;
            _logger = logger;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginatedItemsResponse<ProductDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Items(PaginatedItemsRequest<ProductTypeFilter> request)
        {
            var result = await _catalogService.GetProductsAsync(request.PageIndex, request.PageSize, request.Filters);

            _logger.LogInformation($"{result.Count} products has found");

            return Ok(result);
        }
    }
}
