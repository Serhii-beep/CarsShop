using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Models;
using CarShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CarShop.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {
    //    RoleManager<IdentityRole> _roleManager;
    //    UserManager<User> _userManager;
    //    public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    //    {
    //        _roleManager = roleManager;
    //        _userManager = userManager;
    //    }

    //    public IActionResult Index() => View(_roleManager.Roles.ToList());

    //    public IActionResult Create() => View();

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create(string name)
    //    {
    //        if (!User.IsInRole("admin"))
    //        {
    //            return RedirectToAction("Index", "Cars");
    //        }
    //        if (!string.IsNullOrEmpty(name))
    //        {
    //            var result = await _roleManager.CreateAsync(new IdentityRole(name));
    //            if (result.Succeeded)
    //            {
    //                return RedirectToAction("Index");
    //            }
    //            else
    //            {
    //                foreach (var error in result.Errors)
    //                {
    //                    ModelState.AddModelError(string.Empty, error.Description);
    //                }
    //            }
    //        }
    //        return View(name);
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Delete(string id)
    //    {
    //        if (!User.IsInRole("admin"))
    //        {
    //            return RedirectToAction("Index", "Cars");
    //        }
    //        IdentityRole role = await _roleManager.FindByIdAsync(id);
    //        if (role != null)
    //        {
    //            await _roleManager.DeleteAsync(role);
    //        }
            
    //        return RedirectToAction("Index");
    //    }

    //    public async Task<IActionResult> Edit(string userId)
    //    {
    //        User user = await _userManager.FindByIdAsync(userId);
    //        if (user != null)
    //        {
    //            var userRoles = await _userManager.GetRolesAsync(user);
    //            var allRoles = _roleManager.Roles.ToList();
    //            ChangeRoleViewModel model = new ChangeRoleViewModel
    //            {
    //                UserId = user.Id,
    //                UserEmail = user.Email,
    //                UserRoles = userRoles,
    //                AllRoles = allRoles
    //            };
    //            return View(model);
    //        }
    //        return NotFound();
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(string userId, List<string> roles)
    //    {
    //        User user = await _userManager.FindByIdAsync(userId);
    //        if (user != null)
    //        {
    //            var userRoles = await _userManager.GetRolesAsync(user);
    //            var addedRoles = roles.Except(userRoles);
    //            var removedRoles = userRoles.Except(roles);

    //            await _userManager.AddToRolesAsync(user, addedRoles);

    //            await _userManager.RemoveFromRolesAsync(user, removedRoles);

    //            return RedirectToAction("Index", "Users");
    //        }
    //        return NotFound();
    //    }
    }
}
