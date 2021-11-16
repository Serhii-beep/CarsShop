using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace CarShop.Controllers
{
    [Authorize]
    public class WarehousesController : Controller
    {

        private readonly DbCarShopContext _context;

        public WarehousesController(DbCarShopContext context)
        {
            _context = context;
        }
        // GET: WarehousesController
        public async Task<IActionResult> Index()
        {
            return View(await _context.Warehouses.ToListAsync());
        }

        // GET: WarehousesController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses.Where(c => c.WarehouseId == id).FirstOrDefaultAsync();

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
                _context.Add(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // GET: WarehousesController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses.Where(c => c.WarehouseId == id).FirstOrDefaultAsync();

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
                    _context.Update(warehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseExists(warehouse.WarehouseId))
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses.Where(c => c.WarehouseId == id).FirstOrDefaultAsync();

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
            Warehouse warehouse = await _context.Warehouses.FindAsync(Warehouseid);
            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool WarehouseExists(int id)
        {
            return _context.Warehouses.Any(e => e.WarehouseId == id);
        }
    }
}
