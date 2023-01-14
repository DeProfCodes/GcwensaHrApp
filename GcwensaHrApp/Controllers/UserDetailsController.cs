using GcwensaHrApp.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using GcwensaHrApp.BusinessLogic;
using GcwensaHrApp.ViewModels;
using GcwensaHrApp.Helpers;
using GcwensaHrApp.Enums;
using Microsoft.EntityFrameworkCore.Storage;
using GcwensaHrApp.Data;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Globalization;
using GcwensaHrApp.ViewModels.Account;

namespace GcwensaHrApp.Controllers
{
    public class UserDetailsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserDetailsController> _logger;
        private readonly IUserDataManagement _usersIO;
        private readonly ILeaveRequestDataManagement _leaveRequestIO;
        private readonly ApplicationDbContext _dbContext;

        public UserDetailsController(UserManager<ApplicationUser> userManager, ILogger<UserDetailsController> logger, IUserDataManagement usersIO, ILeaveRequestDataManagement leaveRequestIO, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _logger = logger;
            _usersIO = usersIO;
            _leaveRequestIO = leaveRequestIO;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> UsersManagement()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

            var allUserDetails = await _usersIO.GetAllUsersDetails();
            var allLeavesAvailable = await _leaveRequestIO.GetAllLeavesAvailable();

            foreach (var user in allUserDetails)
            { 
                var leaveDays = allLeavesAvailable.FirstOrDefault(x => x.UserId == user.Id)?.LeaveDaysAvailable ?? 0;
                user.LeaveDaysAvailable = leaveDays;
            }

            var userManagementVm = new UserManagementViewModel
            {
                UserDetails = allUserDetails
            };

            return View(userManagementVm);    
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View(new UserDetailsViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDetailsViewModel userViewModel)
        {
            IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var userId = await _usersIO.CreateUser(userViewModel);
                await _leaveRequestIO.CreateLeaveAvailable(userId, userViewModel.LeaveDaysAvailable);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            return RedirectToAction("UsersManagement", "UserDetails");
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string userId)
        {
            var user = await _usersIO.GetUserByUserId(userId);
            var userRole = await _usersIO.GetUserRole(userId);
            var userLeaveAvailable = await _leaveRequestIO.GetUserLeaveAvailable(userId);   

            var userDtailsVm = ViewModelHelper.CreateUserViewModel(user, userRole);
            userDtailsVm.LeaveDaysAvailable = userLeaveAvailable?.LeaveDaysAvailable ?? 0;

            return View(userDtailsVm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserDetailsViewModel userViewModel)
        {
            IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var userPassword = await _usersIO.EditUser(userViewModel);
                await _leaveRequestIO.EditLeaveAvailable(userViewModel.Id, userViewModel.LeaveDaysAvailable);

                transaction.Commit();
                userViewModel.ResetPassword = true;
                if (userViewModel.ResetPassword)
                {
                    return RedirectToAction("CreateOrEditUserDetails", "UserDetails", new { userId = userViewModel.Id, password = userPassword });
                }
                else
                {
                    return RedirectToAction("UsersManagement", "UserDetails");
                }
            }
            catch (Exception e)
            {
                transaction.Rollback();

                return RedirectToAction("EditUser", "UserDetails", new { userId = userViewModel.Id });
            }
        }

        public async Task<IActionResult> CreateOrEditUserDetails(string userId, string password)
        {
            var user = await _usersIO.GetUserByUserId(userId);

            var userInfoVm = new CreateOrEditUserDetailsViewModel
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Username = user.UserName,
                Password = password
            };

            return View(userInfoVm); 
        }

        public async Task<IActionResult> SuspendUser(string userId)
        {
            await _usersIO.UpdateUserAccountStatus(userId, AccountStatus.Suspended);

            return RedirectToAction("UsersManagement", "UserDetails");
        }

        public async Task<IActionResult> DeleteUser(string userId)
        {
            await _usersIO.UpdateUserAccountStatus(userId, AccountStatus.Deleted);

            return RedirectToAction("UsersManagement", "UserDetails");
        }

        public async Task<IActionResult> SuperDeleteUser(string userId)
        {
            IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                await _usersIO.SuperDeleteUser(userId);

                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback(); 
            }
            return RedirectToAction("UsersManagement", "UserDetails");
        }
    }
}
