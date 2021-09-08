using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
namespace CarShop.Repository
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);
        Task<IEnumerable<Order>> GetOrders();
        Order GetOrder(int id);
        void DeleteOrder(int id);
        void UpdateOrder(Order order);
    }
}
