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
        public CarsController(DbCarShopContext context)
        {
            _context = context;

        }

        // GET: Cars
        public async Task<IActionResult> Index(int? categoryId, int? minPrice, int? maxPrice, int? producerId, int? year)
        {
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Name");

            ViewBag.categoryId = categoryId;
            ViewBag.Path = HttpContext.Request.Path + HttpContext.Request.QueryString;

            IEnumerable<Car> cars = await _context.Cars.Include(c => c.Producer).ToListAsync();

            if (minPrice > maxPrice)
            {
                ModelState.AddModelError("PriceError", "Max price less than min price");
            }
            else
            {
                cars = GetFiltredCars(cars, minPrice, maxPrice, producerId, year);
            }

            if (categoryId == null)
            {
                return View(cars);
            }
            else
            {
                ViewBag.CategoryName = _context.Categories.Find(categoryId).Name;
                return View(cars.Where(c=>c.CategoryId == categoryId));
            }
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id, int? categoryId)
        {
            ViewBag.Path = HttpContext.Request.Path + HttpContext.Request.QueryString;
            ViewBag.categoryId = categoryId;
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.Where(c => c.CarId == id).Include(c => c.Category).Include(c => c.Producer).FirstOrDefaultAsync();
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create(int? categoryId)
        {
            ViewBag.catId = categoryId;
            if (categoryId != null)
            {
                ViewBag.categoryName = _context.Categories.Find(categoryId).Name;
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerFullName");
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Name");
            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? catId, [Bind("CarId,Model,CategoryId,Price,Year,ProducerId,PhotoUrl,Description,OrderId")] Car car)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(car);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index), new { categoryId = catId });
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", car.CategoryId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerFullName", car.OrderId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Name", car.ProducerId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id, int? categoryId)
        {
            ViewBag.catId = categoryId;
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.Where(c => c.CarId == id).Include(c => c.Category).Include(c => c.Producer).FirstOrDefaultAsync();

            if (car == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", car.CategoryId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerFullName", car.OrderId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Name", car.ProducerId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int CarId, int? catId, [Bind("CarId,Model,CategoryId,Price,Year,ProducerId,PhotoUrl,Description,OrderId")] Car car)
        {
            if (CarId != car.CarId)
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
                return RedirectToAction(nameof(Index), new { categoryId = catId});
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", car.CategoryId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerFullName", car.OrderId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Name", car.ProducerId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id, int? categoryId)
        {
            ViewBag.categoryId = categoryId;
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.Where(c => c.CarId == id).Include(c => c.Category).Include(c => c.Producer).FirstOrDefaultAsync();

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CarId, int? categoryId)
        {
            Car car = await _context.Cars.FindAsync(CarId);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { categoryId});
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }

        private IEnumerable<Car> GetFiltredCars(IEnumerable<Car> cars,int? minPrice, int? maxPrice, int? producerId, int? year)
        {
            if (producerId != null)
            {
                cars = cars.Where(c => c.ProducerId == producerId);
            }
            if (year != null)
            {
                cars = cars.Where(c => c.Year == year);
            }
            if (minPrice != null)
            {
                cars = cars.Where(c => c.Price >= minPrice);
            }
            if(maxPrice != null)
            {
                cars = cars.Where(c => c.Price <= maxPrice);
            }

            return cars;
        }
    }
}
