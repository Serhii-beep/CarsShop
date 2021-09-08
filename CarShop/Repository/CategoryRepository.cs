using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Repository
{
    public class CategoryRepository: ICategoryRepository
    {
        DbCarShopContext _context;
        public CategoryRepository(DbCarShopContext context)
        {
            _context = context;
        }

        public async void AddCategory(Category category)
        {
            try
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public Category GetCategory(int id)
        {
            Category category = _context.Categories.Where(a => a.CategoryId == id).FirstOrDefault();
            return category;
        }

        public async void UpdateCategory(Category category)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(ex.Message);
            }
        }

        public async void DeleteCategory(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            try
            {
                _context.Remove(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }


    }
}
