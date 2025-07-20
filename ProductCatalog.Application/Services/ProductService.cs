using Bogus;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain;
using ProductCatalog.Infrastructure;

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
            return await _db.Products
                .Where(p => EF.Functions.ILike(p.Name, $"%{searchQuery}%") ||
                            EF.Functions.ILike(p.Description, $"%{searchQuery}%") ||
                            EF.Functions.ILike(p.Brand, $"%{searchQuery}%") ||
                            EF.Functions.ILike(p.Category, $"%{searchQuery}%") ||
                            EF.Functions.ILike(p.SKU, $"%{searchQuery}%") ||
                            EF.Functions.ILike(p.AvaiabilityStatus, $"%{searchQuery}%"))
                .ToListAsync();
        }
    }
}
