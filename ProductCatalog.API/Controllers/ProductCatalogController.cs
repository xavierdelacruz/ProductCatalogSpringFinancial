using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Interfaces;

namespace ProductCatalog.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCatalogController : Controller
    {
        private readonly IProductService _productService;
        public ProductCatalogController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts(int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new
                {
                    error = "Page and pageSize must be greater than zero."
                });
            }

            try
            {
                var products = await _productService.GetAllProductsAsync(page, pageSize);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An unexpected error occurred.",
                    details = ex.Message
                });
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string query, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest(new
                {
                    error = "Query parameter is required."
                });
            }

            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new
                {
                    error = "Page and pageSize must be greater than zero."
                });
            }

            try
            {
                var products = await _productService.SearchProductsAsync(query, page, pageSize);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An unexpected error occurred.",
                    details = ex.Message
                });
            }
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateProducts(int count)
        {
            try
            {
                await _productService.GenerateProductsAsync(count);
                return NoContent();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new
                {
                    error = ex.Message,
                    parameter = ex.ParamName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An unexpected error occurred.",
                    details = ex.Message
                });
            }
        }
    }
}