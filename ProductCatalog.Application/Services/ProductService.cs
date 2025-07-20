using Bogus;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Domain;
using ProductCatalog.Infrastructure;

namespace ProductCatalog.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly CatalogDbContext _db;

        public ProductService(CatalogDbContext db)
        {
            _db = db;
        }

        public async Task GenerateProductsAsync(int count)
        {

            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be at least 1.");

            if (count > 1000)
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot exceed 1000.");

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

        public async Task<IEnumerable<Product>> SearchProductsAsync(
            string searchQuery,
            int page, 
            int pageSize)
        {
            var queryable = _db.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                queryable = queryable.Where(p =>
                    p.Name.ToLower().Contains(searchQuery) ||
                    p.Description.ToLower().Contains(searchQuery) ||
                    p.Brand.ToLower().Contains(searchQuery) ||
                    p.Category.ToLower().Contains(searchQuery) ||
                    p.SKU.ToLower().Contains(searchQuery) ||
                    p.AvaiabilityStatus.ToLower().Contains(searchQuery)
                );
            }

            return await queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
