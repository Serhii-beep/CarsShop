using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
namespace CarShop.Repository
{
    public interface IOrderRepository
    {
        Task AddOrder(Order order);
        Task<IEnumerable<Order>> GetOrders();
        Order GetOrder(int id);
        Task DeleteOrder(int id);
        Task UpdateOrder(Order order);
    }
}
