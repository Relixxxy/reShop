using System.Net;
using Infrastructure.Identity;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Basket.Host.Services.Interfaces;
using Infrastructure.Models.Responses;
using Basket.Host.Models;
using Infrastructure.Models.Requests;

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
    [ProducesResponseType(typeof(ItemsResponse<Product>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProducts(ItemRequest<string> request)
    {
        var result = await _basketService.GetProducts(request.Item);
        return Ok(new ItemsResponse<Product> { Items = result });
    }
}
