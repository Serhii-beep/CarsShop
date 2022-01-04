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
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly DbCarShopContext _context;

        public WarehouseRepository(DbCarShopContext context)
        {
            _context = context;
        }
        public void Add(Warehouse entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Warehouses.Add(entity);
        }

        public void Delete(int id)
        {
            Warehouse warehouse = _context.Warehouses.FirstOrDefault(wh => wh.WarehouseId == id);
            if (warehouse == null)
            {
                throw new KeyNotFoundException(nameof(warehouse));
            }
            _context.Warehouses.Remove(warehouse);
        }

        public bool Exists(int id)
        {
            return _context.Warehouses.Any(e => e.WarehouseId == id);
        }

        public IEnumerable<Warehouse> GetAll()
        {
            return _context.Warehouses.ToList();
        }

        public Warehouse GetById(int id)
        {
            return _context.Warehouses.FirstOrDefault(wh => wh.WarehouseId == id);
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

        public void Update(Warehouse entity)
        {
            Warehouse warehouse = _context.Warehouses.FirstOrDefault(wh => wh.WarehouseId == entity.WarehouseId);
            if (warehouse == null)
            {
                throw new KeyNotFoundException(nameof(entity));
            }
            try
            {
                _context.Warehouses.Update(entity);
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
