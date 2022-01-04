using System;
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
using CarShop.DAL.Data;
using CarShop.DOM.Repositories;
using CarShop.DOM.Database;

namespace CarShop.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private readonly ICarRepository _carRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProducerRepository _producerRepository;
        private readonly IWarehouseRepository _warehouseRepository;

        private readonly IFileManager _fileManager;
        public CarsController(ICarRepository carRepository, IFileManager fileManager, ICategoryRepository categoryRepository,
            IProducerRepository producerRepository, IWarehouseRepository warehouseRepository)
        {
            _carRepository = carRepository;
            _fileManager = fileManager;
            _categoryRepository = categoryRepository;
            _producerRepository = producerRepository;
            _warehouseRepository = warehouseRepository;
        }
        // GET: Cars
        [AllowAnonymous]
        public IActionResult Index(int? categoryId)
        {
            ViewBag.categoryId = categoryId;
            ViewBag.Path = HttpContext.Request.Path + HttpContext.Request.QueryString;
            IEnumerable<Car> cars = _carRepository.GetAll();
            if (categoryId != null)
            {
                cars = cars.Where(c => c.CategoryId == categoryId);
            }
            ViewBag.CategoryName = _categoryRepository.GetById(categoryId ?? default(int))?.Name;
            return View(cars);
        }
                       



        // GET: Cars/Details/5
        [AllowAnonymous]
        public IActionResult Details(int id, int? categoryId)
        {
            ViewBag.Path = HttpContext.Request.Path + HttpContext.Request.QueryString;
            ViewBag.categoryId = categoryId;

            Car car = _carRepository.GetByIdWithCategoryProducer(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(new CarDetailsViewModel { car=car, RelatedCars = _carRepository.GetRandomCarsByThis(car, 4)});
        }

        // GET: Cars/Create
        public IActionResult Create(int? categoryId)
        {
            ViewBag.catId = categoryId;
            if (categoryId != null)
            {
                ViewBag.categoryName = _categoryRepository.GetById(categoryId ?? default(int))?.Name;
            }

            ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name");
            ViewData["ProducerId"] = new SelectList(_producerRepository.GetAll(), "ProducerId", "Name");
            ViewData["WarehouseId"] = new SelectList(_warehouseRepository.GetAll(), "WarehouseId", "Address");
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
                    _carRepository.Add(car);
                    await _carRepository.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index), new { categoryId = catId });
            }
            ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name", car.CategoryId);
            ViewData["ProducerId"] = new SelectList(_producerRepository.GetAll(), "ProducerId", "Name", car.ProducerId);
            ViewData["WarehouseId"] = new SelectList(_warehouseRepository.GetAll(), "WarehouseId", "Address", car.WarehouseId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public IActionResult Edit(int? id, int? categoryId)
        {

            ViewBag.catId = categoryId;

            if (id == null)
            {
                return NotFound();
            }

            Car car = _carRepository.GetByIdWithCategoryProducer(id ?? default(int));

            if (car == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name", car.CategoryId);
            ViewData["ProducerId"] = new SelectList(_producerRepository.GetAll(), "ProducerId", "Name", car.ProducerId);
            ViewData["WarehouseId"] = new SelectList(_warehouseRepository.GetAll(), "WarehouseId", "Address", car.WarehouseId);
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

            if(uploadFile == null)
            {
                ModelState.AddModelError("PhotoNullError", "Please upload the photo");
            }
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
                    _carRepository.Update(car);
                    await _carRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_carRepository.Exists(car.CarId))
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
            ViewData["CategoryId"] = new SelectList(_categoryRepository.GetAll(), "CategoryId", "Name", car.CategoryId);
            ViewData["ProducerId"] = new SelectList(_producerRepository.GetAll(), "ProducerId", "Name", car.ProducerId);
            ViewData["WarehouseId"] = new SelectList(_warehouseRepository.GetAll(), "WarehouseId", "Address", car.WarehouseId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public IActionResult Delete(int? id, int? categoryId)
        {

            ViewBag.categoryId = categoryId;
            if (id == null)
            {
                return NotFound();
            }

            Car car = _carRepository.GetByIdWithCategoryProducer(id ?? default(int));

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
            Car car = _carRepository.GetById(CarId);
            _fileManager.deleteFile(car.PhotoUrl);
            _carRepository.Delete(CarId);
            await _carRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { categoryId});
        }  
    }
}
