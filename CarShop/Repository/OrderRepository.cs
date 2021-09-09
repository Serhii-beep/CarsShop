using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace CarShop.Repository
{
    public class OrderRepository: IOrderRepository
    {
        DbCarShopContext _context;

        public OrderRepository(DbCarShopContext context)
        {
            _context = context;
        }

        public async Task AddOrder(Order order)
        {
            try
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Order GetOrder(int id)
        {
            Order order = _context.Orders.Where(a => a.OrderId == id).FirstOrDefault();
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }


        public async Task UpdateOrder(Order order)
        {

            try
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(ex.Message);
            }
        }

        public async Task DeleteOrder(int id)
        {
            Order order = await _context.Orders.FirstOrDefaultAsync(a => a.OrderId == id);
            try
            {
                _context.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }
    }
}
