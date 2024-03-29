﻿using GcwensaHrApp.BusinessLogic;
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
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly ILogger<LeaveRequestsController> _logger;
        private readonly IUserDataManagement _usersIO;
        private readonly ILeaveRequestDataManagement _leaveRequestIO;

        public LeaveRequestsController(UserManager<ApplicationUser> userManager, ILogger<LeaveRequestsController> logger, RoleManager<ApplicationRole> roleManager, 
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
            //return Json(new { data = "successed" });
        }

        [HttpGet]
        public async Task<IActionResult> EditLeaveRequest(int leaveId)
        {
            var leaveReq = await _leaveRequestIO.GetLeaveRequestById(leaveId);

            var leaveReqVm = new LeaveRequestViewModel
            {
                LeaveId = leaveId,  
                LeaveType = leaveReq.LeaveType,
                LeaveReason = leaveReq.LeaveReason,
                LeaveStartDate = leaveReq.LeaveStartDate,
                LeaveEndDate = leaveReq.LeaveEndDate,
            };

            return View(leaveReqVm);  
        }

        [HttpPost]
        public async Task<IActionResult> EditLeaveRequest(LeaveRequestViewModel leaveRequest)
        {
            await _leaveRequestIO.EditLeaveRequest(leaveRequest);

            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> CancelLeaveRequest(int leaveId)
        {
            await _leaveRequestIO.CancelLeaveRequest(leaveId);

            return Ok();
            //return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> DeleteLeaveRequest(int leaveId)
        {
            await _leaveRequestIO.DeleteLeaveRequest(leaveId);

            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> SuperDeleteLeaveRequest(int leaveId)
        {
            await _leaveRequestIO.SuperDeleteLeaveRequest(leaveId);

            return Ok();
            //return RedirectToAction("Index", "Dashboard");
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

        public async Task<IActionResult> RejectLeaveRequest(int leaveId)
        {
            await _leaveRequestIO.RejectLeaveRequest(leaveId);

            return Ok();
            //return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> ApproveLeaveRequest(int leaveId)
        {
            await _leaveRequestIO.ApproveLeaveRequest(leaveId);

            return Ok();
            //return RedirectToAction("Index", "Dashboard");
        }
        #endregion


    }
}
