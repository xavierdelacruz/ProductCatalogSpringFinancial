using ProductCatalog.Domain;

namespace ProductCatalog.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(int page, int pageSize);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchQuery, int page, int pageSize);
        Task GenerateProductsAsync(int count);
    }
}
