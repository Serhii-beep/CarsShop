using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
using CarShop.ViewModels;

namespace CarShop.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;
        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index() => View(_userManager.Users.ToList());
    }
}
