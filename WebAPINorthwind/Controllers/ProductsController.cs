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

            var resultado = (

                from p in _context.Products
                from md in _context.Movementdetails
                from m in _context.Movements
                where p.ProductId == md.ProductId
                where m.MovementId == md.MovementId &&
                m.Date.Year == 1996
                select new
                {
                    Producto = p.ProductName, 
                    Cantidad = md.Quantity,
                    Trimestre = (m.Date.Month - 1)/ 3,
                })
                .OrderByDescending(p => p.Cantidad)
                .Take(5);
            return resultado;            
        }
        
        // GET: api/Products/GetTop5Month
        [HttpGet]
        [Route("Top5ByMonth")]
        public IEnumerable<object> GetTop5Month()
        {
            var resultado = (

                from p in _context.Products
                from md in _context.Movementdetails
                from m in _context.Movements
                where p.ProductId == md.ProductId
                where m.MovementId == md.MovementId &&
                m.Date.Year == 1997 && m.Date.Month == 06
                select new
                {
                    Producto = p.ProductName,
                    UnidadPrecio = p.UnitPrice,
                    Cantidad = md.Quantity,
                    MesJunio = m.Date,
                })
                .OrderByDescending(md => md.Cantidad)
                .Take(10);
            return resultado;
        }
        // GET: api/Products/Top5ByWareHouse
        [HttpGet]
        [Route("Top5ByWareHouse")]
        public IEnumerable<object> GetTop5WareHouse()
        {
            return _context.Products
                .Where(w => w.CompanyId == 1)
                .Join(_context.Warehouseproducts,
                p => p.ProductId,
                wp => wp.ProductId,
                (p, wp) => new
                {
                    producto = p.ProductName,
                    Unidad_Stock = wp.UnitsInStock

                })
                .Select(w => new
                {
                    Productos = w.producto,
                    UnidadesStock = w.Unidad_Stock,
                })
                .OrderByDescending(w => w.UnidadesStock)
                .Take(15);
        }
        [HttpGet]
        [Route("Top5ByWareHouseasc")]
        public IEnumerable<object> GetTop5WareHouseasc()
        {

            var resultado = (
                from p in _context.Products
                from wp in _context.Warehouseproducts
                from w in _context.Warehouses
                where p.ProductId == wp.ProductId
                where w.WarehouseId == wp.WarehouseId &&
                w.CompanyId == 1
                select new
                {
                    Producto = p.ProductName,
                    Precio = p.UnitPrice,
                    UnidadesStock = wp.UnitsInStock,
                    Descripcion = w.Description
                })
                .OrderBy(w => w.UnidadesStock)
                .Take(15);
            return resultado;
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
