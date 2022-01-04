
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CarShop.FileManager;
using Microsoft.AspNetCore.Authorization;
using CarShop.DOM.Repositories;
using CarShop.DOM.Database;

namespace CarShop.Controllers
{
    [Authorize]
    public class ProducersController : Controller
    {
        private readonly IProducerRepository _producerRepository;

        private readonly IFileManager _fileManager;

        public ProducersController(IProducerRepository producerRepository, IFileManager fileManager)
        {
            _producerRepository = producerRepository;
            _fileManager = fileManager;
        }

        // GET: Producers
        public IActionResult Index()
        {
            return View(_producerRepository.GetAll());
        }

        // GET: Producers/Details/5
        [AllowAnonymous]
        public IActionResult Details(int? id, int? CarId, string returnUrl)
        {
            ViewBag.CarId = CarId;
            ViewBag.Path = returnUrl;
            if (id == null)
            {
                return NotFound();
            }

            var producer = _producerRepository.GetById(id ?? default(int));
            if (producer == null)
            {
                return NotFound();
            }

            return View(producer);
        }

        // GET: Producers/Create
        public IActionResult Create()
        {
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
                _producerRepository.Add(producer);
                await _producerRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producer);
        }

        // GET: Producers/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producer = _producerRepository.GetById(id ?? default(int));
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
                    _producerRepository.Update(producer);
                    await _producerRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_producerRepository.Exists(producer.ProducerId))
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
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producer = _producerRepository.GetById(id ?? default(int));

            if (producer == null)
            {
                return NotFound();
            }

            return View(producer);
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int producerId)
        {
            var producer = _producerRepository.GetById(producerId);
            _fileManager.deleteFile(producer.LogoUrl);
            _producerRepository.Delete(producerId);
            await _producerRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
