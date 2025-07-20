using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.Services;
using ProductCatalog.Domain;
using ProductCatalog.Infrastructure;

namespace ProductCatalog.Tests;

public class ProductServiceTests
{
    private CatalogDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new CatalogDbContext(options);
    }

    [Fact]
    public async Task GetAllAsync_Returns_Paginated_Results()
    {
        // Arrange
        var dbContext = CreateInMemoryDbContext();

        dbContext.Products.AddRange(
            new Product("Product 1", "Description 1", 100, 10, "Brand A", "Category A", "EAN1234567890", "In Stock", 4.5),
            new Product("Product 2", "Description 2", 200, 20, "Brand B", "Category B", "EAN0987654321", "Out of Stock", 3.5),
            new Product("Product 3", "Description 3", 300, 30, "Brand C", "Category C", "EAN1122334455", "Pre-order", 5.0)
        );

        await dbContext.SaveChangesAsync();

        var service = new ProductService(dbContext);

        // Act
        var result = await service.GetAllProductsAsync(1, 1);

        // Assert
        Assert.Single(result);
        Assert.Equal("Product 1", result.First().Name); ;
    }

    [Fact]
    public async Task SearchAsync_Returns_Matching_Products()
    {
        // Arrange
        var dbContext = CreateInMemoryDbContext();
        dbContext.Products.AddRange(
            new Product("Product 1", "Description 1", 100, 10, "Brand A", "Category A", "EAN1234567890", "In Stock", 4.5),
            new Product("Product 2", "Description 2", 200, 20, "Brand B", "Category B", "EAN0987654321", "Out of Stock", 3.5),
            new Product("Product 3", "Description 3", 300, 30, "Brand C", "Category C", "EAN1122334455", "Pre-order", 5.0)
        );
        await dbContext.SaveChangesAsync();
        var service = new ProductService(dbContext);

        // Act
        var result = await service.SearchProductsAsync("Description 1");

        // Assert
        Assert.Single(result);
        Assert.Equal("Product 1", result.First().Name);
    }

    [Fact]
    public async Task GenerateProductsAsync_Adds_New_Products()
    {
        // Arrange
        var dbContext = CreateInMemoryDbContext();
        var service = new ProductService(dbContext);

        // Act
        await service.GenerateProductsAsync(5);
        var products = await dbContext.Products.ToListAsync();

        // Assert
        Assert.Equal(5, products.Count);
        Assert.All(products, p => Assert.NotNull(p.Name));
    }   
}
