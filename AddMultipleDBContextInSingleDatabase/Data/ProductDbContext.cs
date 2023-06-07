using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using AddMultipleDBContextInSingleDatabase.Models;
using AddMultipleDBContextInSingleDatabase.Storage;

namespace AddMultipleDBContextInSingleDatabase.Data
{
    
    public class ProductDbContext : ProductDbContext<ProductDbContext>
    {        
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }
    }
  
    public class ProductDbContext<TContext> : DbContext, IProductDbContext
        where TContext : DbContext, IProductDbContext
    {       
        public StorageOption StoreOptions { get; set; }

        public ProductDbContext(DbContextOptions<TContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<Brand> Brand { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (StoreOptions is null)
            {
                StoreOptions = this.GetService<StorageOption>();

                if (StoreOptions is null)
                {
                    throw new ArgumentNullException(nameof(StoreOptions), "ProductDbContext must be configured in the DI system.");
                }
            }
            //If any custom model needed
            modelBuilder.ConfigureProductContext(StoreOptions);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.Options.IsFrozen)
            {
                optionsBuilder.ConfigureWarnings(w => w.Ignore(new EventId[] { RelationalEventId.MultipleCollectionIncludeWarning }));
            }
        }
    }
    public static class ModelBuilderExtention
    {
        public static void ConfigureProductContext(this ModelBuilder modelBuilder, StorageOption storeOptions)
        {
            modelBuilder.Entity<Product>(product =>
            {
                product.ToTable(nameof(Product));
                product.HasKey(x => x.Id);
                product.Property(x => x.Name).HasMaxLength(256);
                product.Property(x => x.Quantity);
            });
        }
    }
}
