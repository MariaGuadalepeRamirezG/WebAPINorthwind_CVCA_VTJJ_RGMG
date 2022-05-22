using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPINorthwind.Data;
using WebAPINorthwind.Models;

namespace WebAPINorthwind.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public ProductsController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Products/GetTop5Quantity
        [HttpGet]
        [Route("Top5ByQuantity")]
        public IEnumerable<object> GetTop5Quantity()
        {
            return  _context.Products
                .Where(p => p.ProductId == p.ProductId)
                .Join(_context.Movementdetails,
                p => p.ProductId,
                md => md.ProductId
                (p, md) => new
                {
                    Productos = p.ProductName,
                    Cantidad = md.Quantity
                })
                .Join(_context.Movements,
                m => m.MovementId,
                md => md.MovementId,
                (m, md) => new
                {
                    fecha = m.Date,
                })
                .Where(m => m.fecha.Year == 1996)
                .GroupBy(p => p.Productos)
                .Select(p => new
                {
                    Producto = p.Key,
                    Cantidad = p.Quantity,
                    Trimestre = (p.fecha.Month - 1) / 3

                })
                .OrderByDescending(p => p.Cantidad)
                .Take(5);
        }
        // GET: api/Products/GetTop5Month
        [HttpGet]
        [Route("Top5ByMonth")]
        public IEnumerable<object> GetTop5Month()
        {
            return _context.Products
                .Where(p => p.ProductId == p.ProductId)
                .Join(_context.Movementdetails,
                p => p.ProductId,
                md => md.ProductId
                (p, md) => new
                {
                    Productos = p.ProductName,
                    Precio = p.UnitPrice,
                    Cantidad = md.Quantity
                })
                .Join(_context.Movements,
                m => m.MovementId,
                md => md.MovementId,
                (m, md) => new
                {
                    fecha = m.Date,
                })
                .Where(m => m.fecha.Year == 1997)
                .GroupBy(p => p.Productos)
                .Select(p => new
                {
                    Productos = p.Key,
                    Precio = p.UnitPrice,
                    Cantidad = p.Quantity,
                    Mes = p.fecha.Month == 06

                })
                .OrderByDescending(p => p.Cantidad)
                .Take(10);
        }
        // GET: api/Products/GetTop5Month
        [HttpGet]
        [Route("Top5ByWareHouse")]
        public IEnumerable<object> GetTop5WareHouse()
        {
            return _context.Products
                .Where(p => p.ProductId == p.ProductId)
                .Join(_context.Warehouseproducts,
                p => p.ProductId,
                wp => wp.ProductId
                (p, wp) => new
                {
                    Productos = p.ProductName,
                    UnidadesStock = wp.UnitsInStock
                })
                .Join(_context.Warehouses,
                w => w.WarehouseId,
                wp => wp.WarehouseId,
                (w, wp) => new
                {
                    Almacen = w.Description,
                    NumAlmacen = w.WarehouseId
                })
                .Where(w => w.NAlmacen == 1 && w.NAlmacen == 3 && w.NAlmacen == 4)
                .GroupBy(p => p.Productos)
                .Select(p => new
                {
                    Productos = p.Key,
                    UnidadesStock = p.UnitsInStock,
                    Almacen = p.Description,
                    NumAlmacen = p.NAlmacen == 1 && p.NAlmacen == 3 && p.NAlmacen == 4

                })
                .OrderByDescending(p => p.UnitsInStock)
                .Take(5);
        }

        // GET: api/Products/5
        [HttpGet("")]
        public async Task<ActionResult<Product>> GetProduct()
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync();

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        /*
        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'NorthwindContext.Products'  is null.");
          }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
