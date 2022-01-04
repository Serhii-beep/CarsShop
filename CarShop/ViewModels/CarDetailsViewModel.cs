using CarShop.DOM.Database;
using System.Collections.Generic;

namespace CarShop.ViewModels
{
    public class CarDetailsViewModel
    {

        public Car car{ get; set; }

        public IEnumerable<Car> RelatedCars { get; set; }
    }
}
