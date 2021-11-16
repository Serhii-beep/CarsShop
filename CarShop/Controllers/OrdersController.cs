﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarShop;
using CarShop.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace CarShop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly DbCarShopContext _context;
        public OrdersController(DbCarShopContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.Include(c=>c.Warehouse).ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Where(c => c.OrderId == id).Include(c => c.Warehouse).FirstOrDefaultAsync();

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
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Address");
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
                _context.Add(order);
                await _context.SaveChangesAsync();
                await AddOrdersToCars(cars, order.OrderId);
                return RedirectToAction("Index", "Cars");
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Where(c => c.OrderId == id).Include(c=> c.Warehouse).FirstOrDefaultAsync();
            if (order == null)
            {
                return NotFound();
            }
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "WarehouseId", "Address");
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
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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


        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        private async Task AddOrdersToCars(List<CartItem> orderedCars, int orderId)
        {
            foreach(var item in orderedCars)
            {
                Car car = await _context.Cars.Where(c => c.CarId == item.car.CarId).FirstOrDefaultAsync();
                car.OrderId = orderId;
            }
            await _context.SaveChangesAsync();
        }
    }
}
