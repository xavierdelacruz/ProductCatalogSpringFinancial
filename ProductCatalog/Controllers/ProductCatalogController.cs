using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Interfaces;

namespace ProductCatalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCatalogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly IProductService _productService;
        public ProductCatalogController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts(int page = 1, int pageSize = 10)
        {
            var products = await _productService.GetAllProductsAsync(page, pageSize);
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string query)
        {
            var products = await _productService.SearchProductsAsync(query);
            return Ok(products);
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateProducts(int count)
        {
            await _productService.GenerateProductsAsync(count);
            return NoContent();
        }
    }
}