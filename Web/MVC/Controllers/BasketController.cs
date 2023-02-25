using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Controllers;

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
    public async Task<IActionResult> AddProduct(Product product)
    {
        await _basketService.AddProduct(product);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveProduct(int id, int availableStock)
    {
        await _basketService.RemoveProduct(id, availableStock);
        return RedirectToAction(nameof(Index));
    }
}
