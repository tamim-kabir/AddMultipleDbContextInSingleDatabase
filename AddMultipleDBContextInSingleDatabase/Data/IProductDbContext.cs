using Microsoft.EntityFrameworkCore;
using AddMultipleDBContextInSingleDatabase.Models;

namespace AddMultipleDBContextInSingleDatabase.Data
{
    public interface IProductDbContext : IDisposable
    {
        public DbSet<Product> Product { get; set; }
        public DbSet<Brand> Brand { get; set; }

    }
}
