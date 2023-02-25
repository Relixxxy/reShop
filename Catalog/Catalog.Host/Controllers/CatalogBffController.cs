﻿using System.Net;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Enums;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;
using Catalog.Host.Services.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
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
        public async Task<IActionResult> Products(PaginatedItemsRequest<ProductTypeFilter> request)
        {
            var result = await _catalogService.GetProductsAsync(request.PageIndex, request.PageSize, request.Filters);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Product(IdRequest request)
        {
            var result = await _catalogService.GetProductAsync(request.Id);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Brands()
        {
            var result = await _catalogService.GetBrandsAsync();
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Types()
        {
            var result = await _catalogService.GetTypesAsync();
            return Ok(result);
        }
    }
}
