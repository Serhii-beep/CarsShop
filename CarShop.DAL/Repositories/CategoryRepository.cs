using CarShop.DAL.Data;
using CarShop.DOM.Database;
using CarShop.DOM.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DbCarShopContext _context;

        public CategoryRepository(DbCarShopContext context)
        {
            _context = context;
        }

        public void Add(Category entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Categories.Add(entity);
        }

        public void Delete(int id)
        {
            Category category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                throw new KeyNotFoundException(nameof(category));
            }
            _context.Categories.Remove(category);
        }

        public bool Exists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public Category GetById(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.CategoryId == id);
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void Update(Category entity)
        {
            Category category = _context.Categories.FirstOrDefault(c => c.CategoryId == entity.CategoryId);
            if (category == null)
            {
                throw new KeyNotFoundException(nameof(entity));
            }
            try
            {
                _context.Categories.Update(entity);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(dbUpdateConcurrencyException.Message, dbUpdateConcurrencyException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DbUpdateException(dbUpdateException.Message, dbUpdateException);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
