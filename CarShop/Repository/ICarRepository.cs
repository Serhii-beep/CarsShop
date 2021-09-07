using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;

namespace CarShop.Repository
{
    public interface ICarRepository
    {
        void AddCar(Car car);
        Car FindCar(int id);
        Task<IEnumerable<Car>> GetAllCars();
        void UpdateCar(Car car);
        void DeleteCar(int id);
    }
}
