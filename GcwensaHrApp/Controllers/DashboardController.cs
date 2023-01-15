using GcwensaHrApp.BusinessLogic;
using GcwensaHrApp.Enums;
using GcwensaHrApp.Models;
using GcwensaHrApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GcwensaHrApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserDataManagement _usersIO;
        private readonly ILeaveRequestDataManagement _leaveRequestIO;

        public DashboardController(UserManager<ApplicationUser> userManager, ILogger<DashboardController> logger, RoleManager<ApplicationRole> roleManager, IUserDataManagement usersIO, ILeaveRequestDataManagement leaveRequestIO)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;

            _usersIO = usersIO;
            _leaveRequestIO = leaveRequestIO;
            //_userManager.AddToRoleAsync(user, "admin");
        }

        private string GetLoggedInEmail()
        {
            return User.Identity.Name;
        }

        public async Task<ApplicationUser> GetLoggedInUser()
        {
            var user = await _usersIO.GetUserByEmail(User.Identity.Name);

            return user;
        }

        //[Authorize(Roles = "Admin,User")]

        public async Task<IActionResult> Index()
        {
            var user = await GetLoggedInUser();

            var leaveRequests = new List<LeaveRequest>();
            var isAdminOrHR = await _userManager.IsInRoleAsync(user, UserRole.Admin.GetDisplayName());
            
            if (isAdminOrHR)
            {
                leaveRequests = await _leaveRequestIO.GetAllLeaveRequests();
            }
            else
            {
                leaveRequests = await _leaveRequestIO.GetAllUserLeaveRequests(user.Id);
            }
            
            var userVM = new UserDetailsViewModel
            {
                Firstname = user?.Firstname ?? "No-1st-Name",
                Lastname = user?.Lastname ?? "No-2nd-Name"
            };

            var dashboardVm = new DashboardViewModel()
            {
                LeaveRequests = leaveRequests,
                UserDetails = userVM
            };

            return View(dashboardVm);
        }



        #region ADMINS USERS
        public async Task<IActionResult> UsersManagement()
        {
            var user = await GetLoggedInUser();
            var authorized = await _userManager.IsInRoleAsync(user, UserRole.Admin.GetDisplayName());
            if (authorized)
            {
                return RedirectToAction("UsersManagement", "UserDetails");
            }
            else
            {
                return Unauthorized();
            }
        }

        public async Task<IActionResult> LeavesManagement()
        {
            var user = await GetLoggedInUser();
            var authorized = await _userManager.IsInRoleAsync(user, UserRole.Admin.GetDisplayName());
            if (authorized)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return Unauthorized();
            }
        }
        #endregion
    }
}
