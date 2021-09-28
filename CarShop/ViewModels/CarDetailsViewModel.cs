using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarShop.ViewModels
{
    public class CarDetailsViewModel
    {

        public Car car{ get; set; }

        public IEnumerable<Car> RelatedCars { get; set; }
    }
}
