using Bogus;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain;
using ProductCatalog.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Services
{
    public class ProductService : IProductService
    {
        public ProductService(CatalogDbContext db)
        {
            _db = db;
        }

        private readonly CatalogDbContext _db;
        public async Task GenerateProductsAsync(int count)
        {
            var faker = new Faker<Product>().CustomInstantiator(f => new Product
            (
                f.IndexFaker + 1,
                f.Commerce.ProductName(),
                f.Commerce.ProductDescription(),
                f.Finance.Amount(10, 1000),
                f.Random.Int(0, 100),
                f.Company.CompanyName(),
                f.Commerce.Categories(1)[0],
                f.Commerce.Ean13(),
                f.PickRandom(new[] { "In Stock", "Out of Stock", "Pre-order" }),
                f.Random.Double(1, 5)
            ));

            var products = faker.Generate(count);
            await _db.Products.AddRangeAsync(products);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(int page, int pageSize)
        {
            return await _db.Products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchQuery)
        {
            return await _db.Products.Where(p => p.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                            p.Description.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                            p.Brand.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                            p.Category.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }
    }
}
