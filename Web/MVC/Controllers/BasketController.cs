using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Controllers;

[Authorize]
public class BasketController : Controller
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _basketService.GetProducts();

        if (products is null)
        {
            return RedirectToAction("Index", "Catalog");
        }

        return View(products);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductVM product)
    {
        await _basketService.AddProduct(product);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveProduct(int id, int amount)
    {
        await _basketService.RemoveProduct(id, amount);
        return RedirectToAction(nameof(Index));
    }
}
