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
    public class EmployeesController : ControllerBase
    {
        private readonly NorthwindContext _context;

        public EmployeesController(NorthwindContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
          if (_context.Employees == null)
          {
              return NotFound();
          }
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/Top5Bysales
        [HttpGet]
        [Route("Top5BySales")]
        public IEnumerable<object> GetTop5BySales()
        {
            return _context.Employees
                .Where(e => e.CompanyId == 1)
                .Join(_context.Movements,
                e => e.EmployeeId,
                m => m.EmployeeId,
                (e, m) => new
                {
                    Empreado = e.FirstName + " " + e.LastName,
                    fecha = m.Date,
                    IdMov = m.MovementId
                }).
                Where(em => em.fecha.Year==1997)
                .Join(_context.Movementdetails,
                em => em.IdMov,
                m => m.MovementId,
                (em,m) => new
                {
                    Empleado =em.Empreado,
                    Cantidad = m.Quantity
                }).
                GroupBy(e => e.Empleado)
                .Select(e => new
                {
                    Empleado = e.Key,
                    ventasTotales = e.Sum(g => g.Cantidad)
                })
                .OrderByDescending(e => e.ventasTotales)
                .Take(5);
        }

        // GET: api/Employees/ByCompany/2
        [HttpGet("{CompanyId}")]
        public IEnumerable<object> GetEmployeeByCompany(int CompanyId)
        {
           return _context.Employees
                .Where(e => e.CompanyId == CompanyId)
                .Select(e => new
                {
                    Name = e.FirstName + " " + e.LastName,
                    HireDate = e.HireDate
                });
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
          if (_context.Employees == null)
          {
              return Problem("Entity set 'NorthwindContext.Employees'  is null.");
          }
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, employee);
        }

        // DELETE: api/Employees/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        /*
         * Obtener los mejores 5 empleados, de acuerdo a las
         * ventas por unidad realizadas en 1997
         * Ejemplo de resultados:
         * Empreado (nombre compreto) ventas(Unidades
         * vendidas de todas las ventas que Atendio.
         */


        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
