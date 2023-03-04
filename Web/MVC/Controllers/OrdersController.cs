using Microsoft.AspNetCore.Mvc;
using MVC.Services.Interfaces;

namespace MVC.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly IOrderService _orderService;
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IActionResult> Index()
    {
        var orders = await _orderService.GetOrdersAsync();
        return View(orders);
    }

    public async Task<IActionResult> CreateOrder()
    {
        var result = await _orderService.CreateOrderAsync();
        return RedirectToAction(nameof(Index));
    }
}
