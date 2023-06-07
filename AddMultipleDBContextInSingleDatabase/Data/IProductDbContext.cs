using Microsoft.EntityFrameworkCore;
using TryToAddMultipleDBContext.Models;

namespace TryToAddMultipleDBContext.Data
{
    public interface IProductDbContext : IDisposable
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Brand> Brand { get; set; }

    }
}
