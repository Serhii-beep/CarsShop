using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class CategoriesApiController : ControllerBase
    {
        private readonly DbCarShopContext _context;
        public CategoriesApiController(DbCarShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetAllCategories()
        {
            return Ok(_context.Categories.ToList());
        }

        [HttpGet("{id}")]
        public ActionResult<Category> GetCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if(category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPut("id")]
        public IActionResult UpdateCategory(int id, Category category)
        {
            if(id != category.CategoryId)
            {
                return BadRequest();
            }
            try
            {
                _context.Update(category);
                _context.SaveChanges();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!_context.Categories.Any(c => c.CategoryId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return Ok(category);
        }

        [HttpPost]
        public ActionResult<Category> AddCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                _context.Add(category);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {

                return new BadRequestObjectResult(ex.Message);
            }
            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

        [HttpDelete("{id}")]
        public ActionResult<Category> DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            try
            {
                _context.Remove(category);
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            return Ok();
        }
    }
}
