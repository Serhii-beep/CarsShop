    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CarShop.FileManager;

namespace CarShop.Controllers
{
    public class ProducersController : Controller
    {
        private readonly DbCarShopContext _context;

        private readonly IFileManager _fileManager;

        public ProducersController(DbCarShopContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
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
        public async Task<IActionResult> Create(IFormFile uploadFile, [Bind("ProducerId,Name,Country,LogoUrl,Info")] Producer producer)
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
                producer.LogoUrl = _fileManager.UploadedFile(uploadFile, "images/Logos");
            }

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
        public async Task<IActionResult> Edit(int Producerid, IFormFile uploadFile, [Bind("ProducerId,Name,Country,LogoUrl,Info")] Producer producer)
        {
            if (Producerid != producer.ProducerId)
            {
                return NotFound();
            }

            if (uploadFile == null) { }
            else if (!_fileManager.HasExtenssion(uploadFile, new string[] { ".jpeg", ".png", ".jpg" }))
            {
                ModelState.AddModelError("PhotoExError", "Valid extenssions of photo are .jpg, .png, .jpeg");
            }
            else
            {
                _fileManager.deleteFile(producer.LogoUrl);
                producer.LogoUrl = _fileManager.UploadedFile(uploadFile, "images/Logos");
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
            _fileManager.deleteFile(producer.LogoUrl);
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
