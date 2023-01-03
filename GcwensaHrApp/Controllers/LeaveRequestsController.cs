using GcwensaHrApp.BusinessLogic;
using GcwensaHrApp.Models;
using GcwensaHrApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GcwensaHrApp.Controllers
{
    public class LeaveRequestsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ILogger<LeaveRequestsController> _logger;
        private readonly IUserDataManagement _usersIO;
        private readonly ILeaveRequestDataManagement _leaveRequestIO;

        public LeaveRequestsController(UserManager<ApplicationUser> userManager, ILogger<LeaveRequestsController> logger, RoleManager<IdentityRole> roleManager, 
                                       IUserDataManagement usersIO, ILeaveRequestDataManagement leaveRequestIO)
        {

            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _usersIO = usersIO; 
            _leaveRequestIO = leaveRequestIO;   
        }

        private string GetLoggedInEmail()
        {
            return User.Identity.Name;
        }

        public async Task<IActionResult> Index()
        {
            var allLeaveRequests = await _leaveRequestIO.GetAllLeaveRequests();

            return View(allLeaveRequests);
        }

        #region Employee Leaves Management
        public IActionResult EmployeeLeaveRequests()
        {
            return View();    
        }

        [HttpGet]
        public IActionResult CreateLeaveRequest()
        {
            return View(new LeaveRequestViewModel());  
        }

        [HttpPost]
        public async Task<IActionResult> CreateLeaveRequest(LeaveRequestViewModel leaveRequest)
        {
            var user = await _usersIO.GetUserByEmail(GetLoggedInEmail());
            
            await _leaveRequestIO.CreateLeaveRequest(user.Id, leaveRequest);

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult EditLeaveRequest(int leaveId)
        {
            return View();  
        }

        public IActionResult DeleteLeaveRequest(int leaveId)
        {
            return View();
        }
        #endregion

        #region HR Leaves Management
        public IActionResult AllLeaveRequests()
        {
            return View();
        }

        public IActionResult ManageLeaveRequest(int leaveId)
        {
            return View();
        }

        #endregion


    }
}
