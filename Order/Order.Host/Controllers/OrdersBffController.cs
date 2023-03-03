using System.Net;
using Infrastructure;
using Infrastructure.Identity;
using Infrastructure.Models.Dtos;
using Infrastructure.Models.Response;
using Infrastructure.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order.Host.Services.Interfaces;

namespace Order.Host.Controllers
{
    [ApiController]
    [Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
    [Route(ComponentDefaults.DefaultRoute)]
    public class OrdersBffController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersBffController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ItemsResponse<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrders()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var result = await _orderService.GetOrdersAsync(userId!);
            return Ok(new ItemsResponse<OrderDto> { Items = result });
        }

        [HttpPost]
        [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOrder()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var result = await _orderService.CreateOrderAsync(userId!);
            return Ok(new AddItemResponse<int?> { Id = result });
        }
    }
}
