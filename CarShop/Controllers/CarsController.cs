using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarShop;
using CarShop.Repository;

namespace CarShop.Controllers
{
    public class CarsController : Controller
    {
        private readonly DbCarShopContext _context;
        private ICarRepository _carRepo;

        public CarsController(DbCarShopContext context, ICarRepository carRepo)
        {
            _context = context;
            _carRepo = carRepo;
        }

        // GET: Cars
        public async Task<IActionResult> Index(int? categoryId)
        {
            ViewBag.Path = HttpContext.Request.Path + HttpContext.Request.QueryString;
            if (categoryId == null)
            {
                return View(await _carRepo.GetAllCars());
            }
            else
            {
                return View(await _carRepo.GetAllCarsByCategory((int)categoryId));
            }
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerFullName");
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Name");
            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,Model,CategoryId,Price,Year,ProducerId,PhotoUrl,Description,OrderId")] Car car)
        {
            if (ModelState.IsValid)
            {
                await _carRepo.AddCar(car);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", car.CategoryId);
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerFullName", car.OrderId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Name", car.ProducerId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _carRepo.FindCar((int)id);
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
        public async Task<IActionResult> Edit(int CarId, [Bind("CarId,Model,CategoryId,Price,Year,ProducerId,PhotoUrl,Description,OrderId")] Car car)
        {
            if (CarId != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _carRepo.UpdateCar(car);
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "OrderId", "CustomerFullName", car.OrderId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Name", car.ProducerId);
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
        public async Task<IActionResult> DeleteConfirmed(int CarId)
        {
            await _carRepo.DeleteCar(CarId);
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}
