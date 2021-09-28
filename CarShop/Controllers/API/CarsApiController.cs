using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarShop;

namespace CarShop.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsApiController : ControllerBase
    {
        private readonly DbCarShopContext _context;

        public CarsApiController(DbCarShopContext context)
        {
            _context = context;
        }

        // GET: api/CarsApi
        [HttpGet]
        public ActionResult<IEnumerable<Car>> GetAllCars()
        {
            return Ok(_context.Cars.Include(a => a.Producer).ToList());
        }

        // GET: api/CarsApi/5
        [HttpGet("{id}")]
        public  ActionResult<Car> GetCar(int id)
        {
            var car = _context.Cars.Include(a => a.Producer).FirstOrDefault(a => a.CarId == id);

            if (car == null)
            {
                return NotFound();
            }

            return Ok(car);
        }

        // PUT: api/CarsApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult UpdateCar(int id, Car car)
        {
            if (id != car.CarId)
            {
                return BadRequest();
            }
            try
            {
                _context.Update(car);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cars.Any(e => e.CarId == id))
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


            return Ok(car);
        }

        // POST: api/CarsApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Car> AddCar(Car car)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                _context.Cars.Add(car);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }

            return CreatedAtAction(nameof(GetCar), new { id = car.CarId }, car);
        }

        // DELETE: api/CarsApi/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCar(int id)
        {
            var car = _context.Cars.Find(id);
            if (car == null)
            {
                return NotFound();
            }
            try
            {
                _context.Cars.Remove(car);
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
