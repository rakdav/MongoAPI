using Lab3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.RateLimiting;

namespace Lab3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private DataContext db;

        public ProductsController(DataContext _db)
        {
            this.db = _db;
        }
        [HttpGet]
        public IAsyncEnumerable<Product> GetProducts()
        {
            return db.Products.AsAsyncEnumerable();
        }
        [HttpGet("{id}")]
        [DisableRateLimiting]
        public async Task<IActionResult> GetProduct(long id)
        {
            Product? p = await db.Products.FindAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }
        [HttpPost]
        public async Task<IActionResult> SaveProduct([FromBody] Product product)
        {
            await db.Products.AddAsync(product);
            await db.SaveChangesAsync();
            return Ok(product);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            Product? p = await db.Products.FindAsync(product.ProductId);
            if (p == null) return NotFound();
            p.CategoryId = product.CategoryId;
            p.SupplierId = product.SupplierId;
            p.Name = product.Name;
            p.Price = product.Price;
            db.Update(p);
            await db.SaveChangesAsync();
            return Ok(p);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            Product? p = await db.Products.FindAsync(id);
            if (p == null) return NotFound();
            db.Products.Remove(p);
            await db.SaveChangesAsync();
            return Ok(p);
        }
        [HttpGet("redirect")]
        public IActionResult Redirect()
        {
            return RedirectToAction(nameof(GetProduct),new {Id=1});
        }
    }
}
