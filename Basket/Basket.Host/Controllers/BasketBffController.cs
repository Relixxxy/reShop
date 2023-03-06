using System.Net;
using Basket.Host.Services.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Infrastructure.Models.Dtos;
using Infrastructure.Models.Requests;
using Infrastructure.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Route(ComponentDefaults.DefaultRoute)]
public class BasketBffController : ControllerBase
{
    private readonly ILogger<BasketBffController> _logger;
    private readonly IBasketService _basketService;

    public BasketBffController(
        ILogger<BasketBffController> logger,
        IBasketService basketService)
    {
        _logger = logger;
        _basketService = basketService;
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddProduct(ItemRequest<BasketProductDto> request)
    {
        _logger.LogInformation(request.ToString());
        var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        await _basketService.AddProductAsync(basketId!, request.Item);
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(typeof(ItemsResponse<BasketProductDto>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProducts()
    {
        var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var result = await _basketService.GetProductsAsync(basketId!);
        return Ok(new ItemsResponse<BasketProductDto> { Items = result });
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> RemoveProduct(AmountProductRequest request)
    {
        var basketId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        await _basketService.RemoveProductAsync(basketId!, request.ProductId, request.Amount);
        return Ok();
    }
}