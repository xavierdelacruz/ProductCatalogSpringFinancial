using ProductCatalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(int page, int pageSize);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchQuery);
        Task GenerateProductsAsync(int count);
    }
}
