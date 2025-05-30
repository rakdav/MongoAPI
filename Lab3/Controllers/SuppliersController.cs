using Azure;
using Lab3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SuppliersController : ControllerBase
    {
        private DataContext db;
        public SuppliersController(DataContext _db)
        {
            this.db = _db;
        }

        [HttpGet]
        public IAsyncEnumerable<Supplier> GetSuppliers()
        {
            return db.Suppliers.AsAsyncEnumerable();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult?> GetSupplier(long id)
        {
            Supplier? supplier = await db.Suppliers.Include(s => s.Products).FirstOrDefaultAsync(s=>s.SupplierId==id);
            if (supplier != null)
            {
                if (supplier.Products != null)
                {
                    foreach (Product item in supplier.Products)
                    {
                        item.Supplier = null;
                    }
                }
                return Ok(supplier);
            }
            return NotFound();
        }
        [HttpPatch("{id}")]
        public async Task<Supplier?> PatchSupplier(long id,
            JsonPatchDocument<Supplier> patchDoc)
        {
            Supplier? s= await db.Suppliers.FindAsync(id);
            if (s != null)
            {
                patchDoc.ApplyTo(s);
                await db.SaveChangesAsync();
            }
            return s;
        }
    }
}
