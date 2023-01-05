using GcwensaHrApp.BusinessLogic;
using GcwensaHrApp.Models;
using GcwensaHrApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        //[Authorize(Roles = "Admin,User")]

        public async Task<IActionResult> Index()
        {
            var user = await _usersIO.GetUserByEmail(GetLoggedInEmail());

            var leaveRequests = await _leaveRequestIO.GetAllUserLeaveRequests(user.Id);

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
    }
}
