using System.Net;
using Infrastructure.Identity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Basket.Host.Services.Interfaces;
using Infrastructure.Models.Responses;
using Infrastructure.Models.Requests;
using Infrastructure.Models.Dtos;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope("basket.basket")]
[Route(ComponentDefaults.DefaultRoute)]
public class BasketController : ControllerBase
{
    private readonly ILogger<BasketBffController> _logger;
    private readonly IBasketService _basketService;

    public BasketController(
        ILogger<BasketBffController> logger,
        IBasketService basketService)
    {
        _logger = logger;
        _basketService = basketService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ItemsResponse<BasketProductDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProducts(ItemRequest<string> request)
    {
        var result = await _basketService.GetProductsAsync(request.Item);
        await _basketService.ClearAsync(request.Item);
        return Ok(new ItemsResponse<BasketProductDto> { Items = result });
    }
}
