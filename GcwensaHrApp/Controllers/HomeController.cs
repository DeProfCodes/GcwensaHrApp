using GcwensaHrApp.BusinessLogic;
using GcwensaHrApp.Enums;
using GcwensaHrApp.Models;
using GcwensaHrApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GcwensaHrApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserDataManagement _usersIO;

        public HomeController(UserManager<ApplicationUser> userManager, ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, IUserDataManagement usersIO)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;

            _usersIO = usersIO;
            //_userManager.AddToRoleAsync(user, "admin");
        }

        private string GetLoggedInEmail()
        {
            return User.Identity.Name;
        }

        //[Authorize(Roles = "Admin,User")]

        public async Task<IActionResult> Index()
        {
            var user = await _usersIO.GetUserByEmail(GetLoggedInEmail());
            
            var userVM = new UserDetailsViewModel
            {
                Firstname = user?.Firstname ?? "No-1st-Name",
                Lastname = user?.Lastname ?? "No-2nd-Name"
            };
            return View(userVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
