using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using CarShop.ViewModels;
using Microsoft.EntityFrameworkCore;
using CarShop.DAL.Data;
using CarShop.DOM.Database;
using CarShop.DOM.Repositories;

namespace CarShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ICarRepository _carRepository;

        public CartController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public ViewResult Index(string returnUrl)
        {
            CartViewModel cvm = new CartViewModel();
            cvm.Cart = GetCart();
            cvm.ReturnUrl = returnUrl;
            return View(cvm);
            
        }
        
        public RedirectToActionResult AddToCart(int carId, string returnUrl)
        {
            Car car = _carRepository.GetByIdWithCategoryProducer(carId);
            if (car != null)
            {
                Cart oldCart = GetCart();
                oldCart.AddCar(car);
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(oldCart));
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int carId, string returnUrl)
        {
            Car car = _carRepository.GetByIdWithCategoryProducer(carId);
            if (car != null)
            {
                Cart oldCart = GetCart();
                oldCart.RemoveCar(car);
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(oldCart));
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult Clear(string returnUrl)
        {
            Cart oldCart = GetCart();
            oldCart.Clear();
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(oldCart));
            return RedirectToAction("Index", new { returnUrl = returnUrl });
        }

        public Cart GetCart()
        {
            string cart = HttpContext.Session.GetString("Cart");
            if(cart == null)
            {
                Cart newCart = new Cart();
                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(newCart));
                return newCart;
            }
            return JsonSerializer.Deserialize<Cart>(cart);
        }
    }
}
