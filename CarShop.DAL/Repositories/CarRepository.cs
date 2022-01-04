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
    public class CarRepository : ICarRepository
    {
        private readonly DbCarShopContext _context;

        public CarRepository(DbCarShopContext context)
        {
            _context = context;
        }

        public void Add(Car entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Cars.Add(entity);
        }

        public void AddOrdersToCars(List<Car> orderedCars, int orderId)
        {
            foreach (var car in orderedCars)
            { 
                car.OrderId = orderId;
            }
        }

        public void Delete(int id)
        {
            Car car = _context.Cars.FirstOrDefault(c => c.CarId == id);
            if(car == null)
            {
                throw new KeyNotFoundException(nameof(car));
            }
            _context.Cars.Remove(car);
        }

        public bool Exists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }

        public IEnumerable<Car> GetAll()
        {
            return _context.Cars.Include(c => c.Producer).ToList();
        }

        public Car GetById(int id)
        {
            return _context.Cars.Where(c => c.CarId == id).Include(c => c.Producer).FirstOrDefault();
        }

        public Car GetByIdWithCategoryProducer(int id)
        {
            return _context.Cars.Where(c => c.CarId == id).Include(c => c.Producer).Include(c => c.Category).Include(c => c.Warehouse).FirstOrDefault();
        }

        public IEnumerable<Car> GetRandomCarsByThis(Car car, int n)
        {
            List<Car> cars = _context.Cars.Where(c => c.CarId != car.CarId
                && c.CategoryId == car.CategoryId
                && c.ProducerId == car.ProducerId).ToList();
            if (cars.Count <= n)
            {
                return cars;
            }
            List<Car> selected = new List<Car>();
            List<int> check = new List<int>();
            var rand = new Random();
            while(selected.Count < n)
            {
                int ind = rand.Next(0, cars.Count);
                if(check.Contains(ind))
                {
                    continue;
                }
                selected.Add(cars[ind]);
                check.Add(ind);
            }
            return selected;
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public void Update(Car entity)
        {
            Car car = _context.Cars.FirstOrDefault(c => c.CarId == entity.CarId);
            if(car == null)
            {
                throw new KeyNotFoundException(nameof(entity));
            }
            try
            {
                _context.Cars.Update(entity);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                throw new DbUpdateConcurrencyException(dbUpdateConcurrencyException.Message, dbUpdateConcurrencyException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DbUpdateException(dbUpdateException.Message, dbUpdateException);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
