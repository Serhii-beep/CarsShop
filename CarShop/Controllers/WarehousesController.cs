
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CarShop.DOM.Repositories;
using CarShop.DOM.Database;

namespace CarShop.Controllers
{
    [Authorize]
    public class WarehousesController : Controller
    {

        private readonly IWarehouseRepository _warehouseRepository;

        public WarehousesController(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }
        // GET: WarehousesController
        public IActionResult Index()
        {
            return View(_warehouseRepository.GetAll());
        }

        // GET: WarehousesController/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = _warehouseRepository.GetById(id ?? default(int));

            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // GET: WarehousesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WarehousesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WarehouseId,Address,Latitude,Longitude")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                _warehouseRepository.Add(warehouse);
                await _warehouseRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // GET: WarehousesController/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = _warehouseRepository.GetById(id ?? default(int));

            if (warehouse == null)
            {
                return NotFound();
            }
            return View(warehouse);
        }

        // POST: WarehousesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Warehouseid, [Bind("WarehouseId,Address,Latitude,Longitude")] Warehouse warehouse)
        {
            if (Warehouseid != warehouse.WarehouseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _warehouseRepository.Update(warehouse);
                    await _warehouseRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_warehouseRepository.Exists(warehouse.WarehouseId))
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
            return View(warehouse);
        }

        // GET: WarehousesController/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = _warehouseRepository.GetById(id ?? default(int));

            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // POST: WarehousesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int Warehouseid)
        {
            _warehouseRepository.Delete(Warehouseid);
            await _warehouseRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
