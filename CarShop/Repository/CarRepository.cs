using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Repository
{
    public class CarRepository : ICarRepository
    {
        private DbCarShopContext _context;
        public CarRepository(DbCarShopContext context)
        {
            _context = context;
        }

        public async  Task AddCar(Car car)
        {
            try
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Car> FindCar(int id)
        {
            Car car = await _context.Cars.Where(c => c.CarId == id).Include(c => c.Category).Include(c => c.Producer).FirstOrDefaultAsync();
            
            return car;
        }

        public async Task<IEnumerable<Car>> GetAllCars()
        {
            return await _context.Cars.Include(c => c.Producer).ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetAllCarsByCategory(int categoryId)
        {
            return await _context.Cars.Where(c => c.CategoryId == categoryId).Include(c => c.Producer).ToListAsync();
        }

        public async Task UpdateCar(Car car)
        {
            try
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(ex.Message);
            }
        }

        public async Task DeleteCar(int id)
        {
            Car car = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == id);
            try
            {
                _context.Remove(car);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }
    }
}
