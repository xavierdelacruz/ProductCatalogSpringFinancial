using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProductCatalog.Infrastructure
{
    public class CatalogDbContextFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ProductCatalog;Username=postgres;Password=yourpassword");
            return new CatalogDbContext(optionsBuilder.Options);
        }
    }
}
