using CarShop.DOM.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarShop.DOM.Repositories
{
    public interface ICarRepository : IRepository<Car>
    {
        //Gets n random cars with same category and producer
        IEnumerable<Car> GetRandomCarsByThis(Car car, int n);

        Car GetByIdWithCategoryProducer(int id);

    }
}
