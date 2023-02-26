using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC.Services;
using MVC.Services.Interfaces;
using MVC.ViewModels;
using MVC.ViewModels.CatalogViewModels;
using MVC.ViewModels.Pagination;

namespace MVC.Controllers;

public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    /*public async Task<IActionResult> Index(string? brandFilterApplied, string? typesFilterApplied, int? page, int? itemsPage)
    {
        page ??= 0;
        itemsPage ??= 6;

        var catalog = await _catalogService.GetCatalogItems(page.Value, itemsPage.Value, brandFilterApplied, typesFilterApplied);
        if (catalog == null)
        {
            return View("Error");
        }

        var info = new PaginationInfo()
        {
            ActualPage = page.Value,
            ItemsPerPage = catalog.Data.Count,
            TotalItems = catalog.Count,
            TotalPages = (int)Math.Ceiling((decimal)catalog.Count / itemsPage.Value)
        };
        var vm = new IndexViewModel()
        {
            Products = catalog.Data,
            Brands = await _catalogService.GetBrands(),
            Types = await _catalogService.GetTypes(),
            PaginationInfo = info
        };

        vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : string.Empty;
        vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : string.Empty;

        return View(vm);
    }*/

    public async Task<IActionResult> Index(string? brandFilterApplied, string? typesFilterApplied, int? page, int? itemsPage)
    {
        page ??= 0;
        itemsPage ??= 6;

        var products = await MockProductService.GetProducts();

        var info = new PaginationInfo()
        {
            ActualPage = page.Value,
            ItemsPerPage = products.Count(),
            TotalItems = products.Count(),
            TotalPages = (int)Math.Ceiling((decimal)products.Count() / itemsPage.Value)
        };
        var vm = new IndexViewModel()
        {
            Products = products,
            Brands = new List<SelectListItem>(),
            Types = new List<SelectListItem>(),
            PaginationInfo = info
        };

        vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : string.Empty;
        vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : string.Empty;

        return View(vm);
    }

    public IActionResult ProductInfo(int id)
    {
        var product = new Product()
        {
            Id = id,
            Name = "Fender American Professional II Jazz Bass",
            Description = "Classic low-top sneakers in white leather with signature swoosh logo.",
            Price = 89.99m,
            Amount = 10,
            PictureUrl = "https://static.nike.com/a/images/t_PDP_1280_v1/f_auto,q_auto:eco/18f94c30-87d2-4027-b0d1-12ef75f9fa5a/air-force-1-07-shoe-ZfwvBz.jpg",
            Type = "Sneakers",
            Brand = "Nike"
        };

        return View(product);
    }
}