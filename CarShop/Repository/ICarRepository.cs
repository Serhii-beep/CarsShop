using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;

namespace CarShop.Repository
{
    public interface ICarRepository
    {
        Task AddCar(Car car);
        Car FindCar(int id);
        Task<IEnumerable<Car>> GetAllCars();
        Task UpdateCar(Car car);
        Task DeleteCar(int id);
    }
}
