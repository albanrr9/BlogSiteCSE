using BlogSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult AddAdmin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAdmin(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "Email is required");
                return View();
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("", $"User with email {email} doesnt exist");
                return View();
            }
            if(await _userManager.IsInRoleAsync(user, "Admin"))
            {
                ModelState.AddModelError("", $"User {email} is already an admin");
                return View();
            }
            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (result.Succeeded)
            {
                ViewBag.Message = $"User {email} has been added to the Admin role!";
                return View();
            }
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ListAdmins()
        {
            // Get all users
            var users = await _userManager.Users.ToListAsync();

            // Filter users with the Admin role
            var adminUsers = new List<ApplicationUser>();
            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    adminUsers.Add(user);
                }
            }

            return View(adminUsers);
        }
    }
}
