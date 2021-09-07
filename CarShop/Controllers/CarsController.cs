using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarShop;

namespace CarShop.Controllers
{
    public class CarsController : Controller
    {
        private readonly DbCarShopContext _context;
        //private readonly ICarRepository carRepository;

        public CarsController(DbCarShopContext context /*ICarRepository _carRepository*/)
        {
            //carRepository = _carRepository;
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var dbCarShopContext = _context.Cars.Include(c => c.Category).Include(c => c.Order).Include(c => c.Producer);
            //var dbCarShopContext = carRepository.GetCars();
            return View(await dbCarShopContext.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Category)
                .Include(c => c.Order)
                .Include(c => c.Producer)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "Address");
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Country");
            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,Model,CategoryId,Price,Year,ProducerId,PhotoUrl,Description,OrderId")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", car.CategoryId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "Address", car.OrderId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Country", car.ProducerId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", car.CategoryId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "Address", car.OrderId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Country", car.ProducerId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,Model,CategoryId,Price,Year,ProducerId,PhotoUrl,Description,OrderId")] Car car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", car.CategoryId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "Address", car.OrderId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Country", car.ProducerId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Category)
                .Include(c => c.Order)
                .Include(c => c.Producer)
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}
