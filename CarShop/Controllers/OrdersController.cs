
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarShop.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using CarShop.DOM.Repositories;
using CarShop.DOM.Database;

namespace CarShop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly ICarRepository _carRepository;
        public OrdersController(IOrderRepository orderRepository, IWarehouseRepository warehouseRepository, ICarRepository carRepository)
        {
            _orderRepository = orderRepository;
            _warehouseRepository = warehouseRepository;
            _carRepository = carRepository;
        }

        // GET: Orders
        public IActionResult Index()
        {
            return View(_orderRepository.GetAll());
        }

        // GET: Orders/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order order = _orderRepository.GetByIdWithWarehouse(id ?? default(int));

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create(string orderedCars, string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            ViewBag.Cars = orderedCars;
            ViewData["WarehouseId"] = new SelectList(_warehouseRepository.GetAll(), "WarehouseId", "Address");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string orderedCars, [Bind("OrderId,CustomerFullName,WarehouseId")] Order order)
        {
            List<CartItem> cars = JsonSerializer.Deserialize<List<CartItem>>(orderedCars);
            if (ModelState.IsValid)
            {
                _orderRepository.Add(order);
                await _orderRepository.SaveChangesAsync();
                await AddOrdersToCars(cars, order.OrderId);
                return RedirectToAction("Index", "Cars");
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order order = _orderRepository.GetByIdWithWarehouse(id ?? default(int));
            if (order == null)
            {
                return NotFound();
            }
            ViewData["WarehouseId"] = new SelectList(_orderRepository.GetAll(), "WarehouseId", "Address");
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Orderid, [Bind("OrderId,CustomerFullName, WarehouseId")] Order order)
        {
            if (Orderid != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _orderRepository.Update(order);
                    await _orderRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_orderRepository.Exists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        private async Task AddOrdersToCars(List<CartItem> orderedCars, int orderId)
        {
            foreach(var item in orderedCars)
            {
                Car car = _carRepository.GetById(item.car.CarId);
                car.OrderId = orderId;
            }
            await _carRepository.SaveChangesAsync();
        }
    }
}
