using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain;

namespace ProductCatalog.Infrastructure
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                    .HasDefaultValueSql("gen_random_uuid()");
            });
        }

        public DbSet<Product> Products => Set<Product>();
    }
}
