using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesApiController : ControllerBase
    {
        private readonly DbCarShopContext _context;
        public WarehousesApiController(DbCarShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Warehouse>> GetAllWarehouses()
        {
            return Ok(_context.Warehouses.Select(a => new { a.WarehouseId, a.Address, a.Latitude, a.Longitude  }).ToList()); 
        }

        [HttpGet("{id}")]
        public ActionResult<Warehouse> GetWarehouse(int id)
        {
            var warehouse = _context.Warehouses.FirstOrDefault(c => c.WarehouseId == id);
            if (warehouse == null)
            {
                return NotFound();
            }
            return Ok(warehouse);
        }

        [HttpPut("id")]
        public IActionResult UpdateWarehouse(int id, Warehouse warehouse)
        {
            if (id != warehouse.WarehouseId)
            {
                return BadRequest();
            }
            try
            {
                _context.Update(warehouse);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Warehouses.Any(c => c.WarehouseId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return Ok(warehouse);
        }

        [HttpPost]
        public ActionResult<Warehouse> AddWarehouse(Warehouse warehouse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                _context.Add(warehouse);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {

                return new BadRequestObjectResult(ex.Message);
            }
            return CreatedAtAction(nameof(GetWarehouse), new { id = warehouse.WarehouseId }, warehouse);
        }

        [HttpDelete("{id}")]
        public ActionResult<Warehouse> DeleteWarehouse(int id)
        {
            var warehouse = _context.Warehouses.FirstOrDefault(c => c.WarehouseId == id);
            if (warehouse == null)
            {
                return NotFound();
            }
            try
            {
                _context.Remove(warehouse);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return Ok();
        }
    }
}
