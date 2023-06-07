using Microsoft.EntityFrameworkCore;
#nullable enable
namespace TryToAddMultipleDBContext.Storage
{
    public class StorageOption
    {    
        public Action<DbContextOptionsBuilder>? ProductDbContext { get; set; }
        public Action<IServiceProvider, DbContextOptionsBuilder>? DbContextOptions { get; set; }
        public string? DefaultSchema { get; set; } = null;
        public bool EnablePooling { get; set; } = false;
        public int? PoolSize { get; set; }
    }
    
}
