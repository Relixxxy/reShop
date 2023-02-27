using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Order()
        {
            return View();
        }
    }
}
