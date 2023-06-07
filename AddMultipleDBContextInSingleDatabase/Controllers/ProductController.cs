using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AddMultipleDBContextInSingleDatabase.Data;
using AddMultipleDBContextInSingleDatabase.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AddMultipleDBContextInSingleDatabase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly ProductDbContext _productDb;
        public ProductController(ApplicationDbContext dbContext, ProductDbContext productDbContext)
        {
            _db = dbContext;
            _productDb = productDbContext;
        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProduct()
        {
            var prodduct = new Product
            {
                Name = "Shirt",
                Quantity = 100,
            };
            _productDb.Product.Add(prodduct);
            await _productDb.SaveChangesAsync();
            var productList = await _productDb.Product.ToListAsync();
            return Ok(productList);
        }       
    }
}
