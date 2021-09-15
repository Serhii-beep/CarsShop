using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Session;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Repository;
using CarShop.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using CarShop.ViewModels;

namespace CarShop.Controllers
{
    public class CartController : Controller
    {
        private ICarRepository _carRepo;

        public CartController(ICarRepository carRepo)
        {
            _carRepo = carRepo;
        }

        public ViewResult Index(string returnUrl)
        {
            CartViewModel cvm = new CartViewModel();
            cvm.Cart = GetCart();
            cvm.ReturnUrl = returnUrl;
            return View(cvm);
            
        }
        
        public async Task<RedirectToActionResult> AddToCart(int carId, string returnUrl)
        {
            Car car = await _carRepo.FindCar(carId);
            if(car != null)
            {
                Cart oldCart = GetCart();
                oldCart.AddCar(car);
                this.HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(oldCart));
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public async Task<RedirectToActionResult> RemoveFromCart(int carId, string returnUrl)
        {
            Car car = await _carRepo.FindCar(carId);
            if(car != null)
            {
                Cart oldCart = GetCart();
                oldCart.RemoveCar(car);
                this.HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(oldCart));
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult Clear(string returnUrl)
        {
            Cart oldCart = GetCart();
            oldCart.Clear();
            this.HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(oldCart));
            return RedirectToAction("Index", new { returnUrl = returnUrl });
        }

        public Cart GetCart()
        {
            string cart = this.HttpContext.Session.GetString("Cart");
            if(cart == null)
            {
                Cart newCart = new Cart();
                this.HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(newCart));
                return newCart;
            }
            return JsonSerializer.Deserialize<Cart>(cart);
        }
    }
}
