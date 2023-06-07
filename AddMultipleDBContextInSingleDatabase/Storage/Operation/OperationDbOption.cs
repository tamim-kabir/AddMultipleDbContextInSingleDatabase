using Microsoft.EntityFrameworkCore;
using AddMultipleDBContextInSingleDatabase.Data;
#nullable enable
namespace AddMultipleDBContextInSingleDatabase.Storage.Operation
{
    public static class OperationDbOption
    {
        public static IServiceCollection AddProductDbContext<TContext>(this IServiceCollection services, Action<StorageOption>? storeOptionsAction = null) 
            where TContext : DbContext, IProductDbContext
        {
            var storeOptions = new StorageOption();
            services.AddSingleton(storeOptions);
            storeOptionsAction?.Invoke(storeOptions);
            if (storeOptions.DbContextOptions != null)
            {
                if (storeOptions.EnablePooling)
                {
                    if (storeOptions.PoolSize.HasValue)
                    {
                        services.AddDbContextPool<TContext>(storeOptions.DbContextOptions,
                            storeOptions.PoolSize.Value);
                    }
                    else
                    {
                        services.AddDbContextPool<TContext>(storeOptions.DbContextOptions);
                    }
                }
                else
                {
                    services.AddDbContext<TContext>(storeOptions.DbContextOptions);
                }
            }
            else
            {
                if (storeOptions.EnablePooling)
                {
                    if (storeOptions.PoolSize.HasValue)
                    {
                        services.AddDbContextPool<TContext>(
                            dbCtxBuilder => { storeOptions.ProductDbContext?.Invoke(dbCtxBuilder); },
                            storeOptions.PoolSize.Value);
                    }
                    else
                    {
                        services.AddDbContextPool<TContext>(
                            dbCtxBuilder => { storeOptions.ProductDbContext?.Invoke(dbCtxBuilder); });
                    }
                }
                else
                {
                    services.AddDbContext<TContext>(dbCtxBuilder =>
                    {
                        storeOptions.ProductDbContext?.Invoke(dbCtxBuilder);
                    });
                }
            }
            services.AddScoped<IProductDbContext>(svcs => svcs.GetRequiredService<TContext>());
            return services;
        }
    }
}
