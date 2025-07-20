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
    public async Task GenerateProductsAsync_WithValidCount_AddsProducts()
    {
        // Arrange
        using var dbContext = CreateInMemoryDbContext();
        var service = new ProductService(dbContext);
        int count = 5;

        // Act
        await service.GenerateProductsAsync(count);
        var products = await dbContext.Products.ToListAsync();

        // Assert
        Assert.Equal(count, products.Count);
        Assert.All(products, p => Assert.False(string.IsNullOrWhiteSpace(p.Name)));
    }

    [Fact]
    public async Task GenerateProductsAsync_WithZeroCount_ThrowsException()
    {
        // Arrange
        using var dbContext = CreateInMemoryDbContext();
        var service = new ProductService(dbContext);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            service.GenerateProductsAsync(0));

        Assert.Equal("Count must be at least 1. (Parameter 'count')", ex.Message);
    }

    [Fact]
    public async Task GenerateProductsAsync_WithTooLargeCount_ThrowsException()
    {
        // Arrange
        using var dbContext = CreateInMemoryDbContext();
        var service = new ProductService(dbContext);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            service.GenerateProductsAsync(5000));

        Assert.Equal("Count cannot exceed 1000. (Parameter 'count')", ex.Message);
    }


    [Fact]
    public async Task SearchProductsAsync_ReturnsPaginatedResults()
    {
        // Arrange
        var dbContext = CreateInMemoryDbContext();
        var service = new ProductService(dbContext);
        dbContext.Products.AddRange(
            new Product("Product 1", "Description 1", 100, 10, "Brand A", "Category A", "EAN1234567890", "In Stock", 4.5),
            new Product("Product 2", "Description 2", 200, 20, "Brand B", "Category B", "EAN0987654321", "Out of Stock", 3.5),
            new Product("Product 3", "Description 3", 300, 30, "Brand C", "Category C", "EAN1122334455", "Pre-order", 5.0)
        );
        await dbContext.SaveChangesAsync();

        string query = "Brand A";
        int page = 1;
        int pageSize = 10;

        // Act
        var result = await service.SearchProductsAsync(query, page, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Brand A", result.First().Brand);
    }
}
