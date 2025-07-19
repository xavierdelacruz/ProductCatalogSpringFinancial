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
