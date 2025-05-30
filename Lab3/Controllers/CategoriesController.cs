using Lab3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : Controller
    {
        public CategoriesController()
        {

        }
        [HttpGet]
        public IAsyncEnumerable<Category> GetCategories()
        {
            return 
        }
        [HttpGet("{id}")]
        public async Task<IActionResult?> GetCategory(long id)
        {
            Category? category = await db.Categories.
                FirstOrDefaultAsync(s => s.CategoryId == id);
            if (category != null)
            { 
                return Ok(category);
            }
            return NotFound();
        }
    }
}
