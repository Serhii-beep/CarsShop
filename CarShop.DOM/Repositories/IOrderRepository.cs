using CarShop.DOM.Database;
using System.Collections.Generic;

namespace CarShop.DOM.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order GetByIdWithWarehouse(int id);
    }
}
