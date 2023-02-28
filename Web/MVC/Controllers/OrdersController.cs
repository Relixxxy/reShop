using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

[Authorize]
public class OrdersController : Controller
{
    public IActionResult Order()
    {
        return View();
    }
}
