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
    public class OrderRepository : IOrderRepository
    {
        private readonly DbCarShopContext _context;

        public OrderRepository(DbCarShopContext context)
        {
            _context = context;
        }

        public void Add(Order entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Orders.Add(entity);
        }

        public void Delete(int id)
        {
            Order order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                throw new KeyNotFoundException(nameof(order));
            }
            _context.Orders.Remove(order);
        }

        public bool Exists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        public IEnumerable<Order> GetAll()
        {
            return _context.Orders.Include(o => o.Warehouse).ToList();
        }

        public Order GetById(int id)
        {
            return _context.Orders.FirstOrDefault(o => o.OrderId == id);
        }

        public Order GetByIdWithWarehouse(int id)
        {
            return _context.Orders.Where(o => o.OrderId == id).Include(o => o.Warehouse).FirstOrDefault();
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

        public void Update(Order entity)
        {
            Order order = _context.Orders.FirstOrDefault(o => o.OrderId == entity.OrderId);
            if (order == null)
            {
                throw new KeyNotFoundException(nameof(entity));
            }
            try
            {
                _context.Orders.Update(entity);
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
