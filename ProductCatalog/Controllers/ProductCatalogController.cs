using Microsoft.AspNetCore.Mvc;

namespace ProductCatalog.Controllers
{
    public class ProductCatalogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
// This code defines a simple ASP.NET Core MVC controller named ProductCatalogController.