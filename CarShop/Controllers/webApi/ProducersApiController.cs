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
    public class ProducersApiController : ControllerBase
    {
        private readonly DbCarShopContext _context;

        public ProducersApiController(DbCarShopContext context)
        {
            _context = context;
        }

        // GET: api/ProducersApi
        [HttpGet]
        public ActionResult<IEnumerable<Producer>> GetAllProducers()
        {
            return Ok(_context.Producers.ToList());
        }

        // GET: api/ProducersApi/5
        [HttpGet("{id}")]
        public ActionResult<Producer> GetProducer(int id)
        {
            var producer =  _context.Producers.Find(id);

            if (producer == null)
            {
                return NotFound();
            }

            return Ok(producer);
        }

        // PUT: api/ProducersApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult UpdateProducer(int id, Producer producer)
        {
            if (id != producer.ProducerId)
            {
                return BadRequest();
            }
            try
            {
                _context.Update(producer);
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Producers.Any(e => e.ProducerId == id))
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

            return Ok(producer);
        }

        // POST: api/ProducersApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public  ActionResult<Producer> AddProducer(Producer producer)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                _context.Producers.Add(producer);
                _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return CreatedAtAction( nameof(GetProducer), new { id = producer.ProducerId }, producer);
        }

        // DELETE: api/ProducersApi/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProducer(int id)
        {
            var producer = _context.Producers.Find(id);
            if (producer == null)
            {
                return NotFound();
            }
            try
            {
                _context.Producers.Remove(producer);
                _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return Ok();
        }
    }
}
