using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TryToAddMultipleDBContext.Data;
using TryToAddMultipleDBContext.Storage.Operation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>((m, o) =>
{
    o.UseSqlServer(m.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"),
       e => e.MigrationsAssembly(typeof(Program).Assembly.FullName));
});
//Secondery db context
builder.Services.AddProductDbContext<ProductDbContext>(so => so.DbContextOptions = DbContextOptions);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //Have a look at Migration Commad.txt for add migration
    using var scope = app.Services.CreateScope();
    await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<ProductDbContext>().Database.MigrateAsync();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void DbContextOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder dbContextOptionsBuilder)
{
    dbContextOptionsBuilder.UseSqlServer(serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection"),
                                        e => e.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name));

}

