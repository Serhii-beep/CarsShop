﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CarShop.ViewModels;
using CarShop.FileManager;
using Microsoft.AspNetCore.Http;
using System.IO;
namespace CarShop.Controllers
{
    public class CarsController : Controller
    {
        private readonly DbCarShopContext _context;

        private readonly IFileManager _fileManager;
        public CarsController(DbCarShopContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        // GET: Cars
        public async Task<IActionResult> Index(int? categoryId, int? minPrice, int? maxPrice, string producerId, int? year)
        {
            ViewBag.categoryId = categoryId;
            ViewBag.Path = HttpContext.Request.Path + HttpContext.Request.QueryString;
            List<SelectListItem> producers = new List<SelectListItem>();
            foreach(var producer in _context.Producers.ToList())
            {
                producers.Add(new SelectListItem { Text = producer.Name, Value = producer.Name });
            }
            ViewBag.Producers = producers;
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

            var car = await _context.Cars.Where(c => c.CarId == id).Include(c => c.Category).Include(c => c.Producer).Include(c=>c.Warehouse).FirstOrDefaultAsync();
            if (car == null)
            {
                return NotFound();
            }

            List<Car> cars = _context.Cars.Where(a => a.CategoryId == car.CategoryId && a.ProducerId == car.ProducerId && a.CarId != car.CarId).ToList();
            GetNRandomCars(cars, 4);

            return View(new CarDetailsViewModel { car=car, RelatedCars = cars});
        }

        // GET: Cars/Create
        public IActionResult Create(int? categoryId)
        {
            if(!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Cars");
            }
            ViewBag.catId = categoryId;
            if (categoryId != null)
            {
                ViewBag.categoryName = _context.Categories.Find(categoryId).Name;
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Address");
            return View();
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? catId, IFormFile uploadFile, [Bind("CarId,Model,CategoryId,Price,Year,ProducerId,PhotoUrl,Description,OrderId,WarehouseId")] Car car)
        {
            if (uploadFile == null)
            {
                ModelState.AddModelError("PhotoNullError", "Please upload the photo");
            }
            else if (!_fileManager.HasExtenssion(uploadFile, new string[] { ".jpeg", ".png", ".jpg" }))
            {
                ModelState.AddModelError("PhotoExError", "Valid extenssions of photo are .jpg, .png, .jpeg");
            }
            else
            {
                car.PhotoUrl = _fileManager.UploadedFile(uploadFile, "images/Transport");
            }

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
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Name", car.ProducerId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Address", car.WarehouseId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id, int? categoryId)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Cars");
            }

            ViewBag.catId = categoryId;

            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.Where(c => c.CarId == id).Include(c => c.Category).Include(c => c.Producer).Include(c=>c.Warehouse).FirstOrDefaultAsync();

            if (car == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", car.CategoryId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Name", car.ProducerId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Address", car.WarehouseId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int CarId, int? catId, IFormFile uploadFile, [Bind("CarId,Model,CategoryId,Price,Year,ProducerId,PhotoUrl,Description,OrderId,WarehouseId")] Car car)
        {
            if (CarId != car.CarId)
            {
                return NotFound();
            }

            if(uploadFile == null){}
            else if (!_fileManager.HasExtenssion(uploadFile,  new string[] { ".jpeg", ".png", ".jpg"}))
            {
                ModelState.AddModelError("PhotoExError", "Valid extenssions of photo are .jpg, .png, .jpeg");
            }
            else
            {
                _fileManager.deleteFile(car.PhotoUrl);
                car.PhotoUrl = _fileManager.UploadedFile(uploadFile, "images/Transport");
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
            ViewData["ProducerId"] = new SelectList(_context.Producers, "ProducerId", "Name", car.ProducerId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Address", car.WarehouseId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id, int? categoryId)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Cars");
            }
            ViewBag.categoryId = categoryId;
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.Where(c => c.CarId == id).Include(c => c.Category).Include(c => c.Producer).Include(c=>c.Warehouse).FirstOrDefaultAsync();

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
            _fileManager.deleteFile(car.PhotoUrl);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { categoryId});
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }

        private List<Car> GetNRandomCars(List<Car> cars, int n )
        {
            if(cars.Count <= n)
            {
                return cars;
            }
            var selected = new List<Car>();
            double needed = n;
            double available = cars.Count;
            var rand = new Random();
            while (selected.Count < n)
            {
                if (rand.NextDouble() < needed / available)
                {
                    selected.Add(cars[(int)available - 1]);
                   needed--;
                }
                available--;
            }

            return selected;
        }
        private IEnumerable<Car> GetFiltredCars(IEnumerable<Car> cars, int? minPrice, int? maxPrice, string producerId, int? year)
        {
            if (!string.IsNullOrEmpty(producerId))
            {
                cars = cars.Where(c => c.Producer.Name.Contains(producerId));
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
