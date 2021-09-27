using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarShop;

namespace CarShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersApiController : ControllerBase
    {
        private readonly DbCarShopContext _context;

        public OrdersApiController(DbCarShopContext context)
        {
            _context = context;
        }

        // GET: api/OrdersApi
        [HttpGet]
        public ActionResult<IEnumerable<Order>> GetOrders()
        {
            return Ok(_context.Orders.ToList());
        }

        // GET: api/OrdersApi/5
        [HttpGet("{id}")]
        public ActionResult<Order> GetOrder(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/OrdersApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }
            try
            {
                _context.Update(order);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Orders.Any(e => e.OrderId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return NoContent();
        }

        // POST: api/OrdersApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Order> PostOrder(Order order)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }

            return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
        }

        // DELETE: api/OrdersApi/5
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            try
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return Ok();
        }


    }
}
