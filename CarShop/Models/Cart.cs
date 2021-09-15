using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CarShop.Models
{
    public class Cart
    {
        [JsonInclude]
        public List<CartItem> CartItems { get; set; }
        public Cart()
        {
            CartItems = new List<CartItem>();
        }

        public void AddCar(Car _car)
        {
            CartItem cartItem = CartItems.Where(c => c.car.CarId == _car.CarId).FirstOrDefault();
            if(cartItem == null)
            {
                CartItems.Add(new CartItem { car = _car });
            }
        }

        public void RemoveCar(Car _car)
        {
            CartItems.RemoveAll(c => c.car.CarId == _car.CarId);
        }

        public int TotalPrice()
        {
            return CartItems.Sum(c => c.car.Price);
        }

        public void Clear()
        {
            CartItems.Clear();
        }
    }
}
