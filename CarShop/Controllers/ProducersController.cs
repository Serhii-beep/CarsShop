﻿    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarShop;

namespace CarShop.Controllers
{
    public class ProducersController : Controller
    {
        private readonly DbCarShopContext _context;

        public ProducersController(DbCarShopContext context)
        {
            _context = context;
        }

        // GET: Producers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Producers.ToListAsync());
        }

        // GET: Producers/Details/5
        public async Task<IActionResult> Details(int? id, int? CarId, string returnUrl)
        {
            ViewBag.CarId = CarId;
            ViewBag.Path = returnUrl;
            if (id == null)
            {
                return NotFound();
            }

            var producer = await _context.Producers.Where(c => c.ProducerId == id).FirstOrDefaultAsync();
            if (producer == null)
            {
                return NotFound();
            }

            return View(producer);
        }

        // GET: Producers/Create
        public IActionResult Create()
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Cars");
            }
            return View();
        }

        // POST: Producers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProducerId,Name,Country,LogoUrl,Info")] Producer producer)
        {

            if (ModelState.IsValid)
            {
                _context.Add(producer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producer);
        }

        // GET: Producers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Cars");
            }
            if (id == null)
            {
                return NotFound();
            }

            var producer = await _context.Producers.Where(c => c.ProducerId == id).FirstOrDefaultAsync();
            if (producer == null)
            {
                return NotFound();
            }
            return View(producer);
        }

        // POST: Producers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Producerid, [Bind("ProducerId,Name,Country,LogoUrl,Info")] Producer producer)
        {
            if (Producerid != producer.ProducerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProducerExists(producer.ProducerId))
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
            return View(producer);
        }

        // GET: Producers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Cars");
            }
            if (id == null)
            {
                return NotFound();
            }

            var producer = await _context.Producers.Where(c => c.ProducerId == id).FirstOrDefaultAsync();

            if (producer == null)
            {
                return NotFound();
            }

            return View(producer);
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Producerid)
        {
            var producer = await _context.Producers.FindAsync(Producerid);
            _context.Producers.Remove(producer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProducerExists(int id)
        {
            return _context.Producers.Any(e => e.ProducerId == id);
        }
    }
}
