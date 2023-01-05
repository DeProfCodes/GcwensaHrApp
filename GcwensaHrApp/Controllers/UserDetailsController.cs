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

namespace GcwensaHrApp.Controllers
{
    public class UserDetailsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<UserDetailsController> _logger;
        private readonly IUserDataManagement _usersIO;
        private readonly ApplicationDbContext _dbContext;

        public UserDetailsController(UserManager<ApplicationUser> userManager, ILogger<UserDetailsController> logger, IUserDataManagement usersIO, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _logger = logger;
            _usersIO = usersIO;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> UsersManagement()
        {
            var allUserDetails = await _usersIO.GetAllUsersDetails();

            var userManagementVm = new UserManagementViewModel
            {
                UserDetails = allUserDetails
            };

            return View(userManagementVm);    
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View(new UserManagementViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDetailsViewModel userViewModel)
        {
            IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                await _usersIO.CreateUser(userViewModel);

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

            var userDtailsVm = ViewModelHelper.CreateUserViewModel(user, userRole);

            return View(userDtailsVm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserDetailsViewModel userViewModel)
        {
            IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
            try
            {
                await _usersIO.EditUser(userViewModel);
                
                transaction.Commit();

                return RedirectToAction("UsersManagement", "UserDetails");
            }
            catch (Exception e)
            {
                transaction.Rollback();

                return RedirectToAction("EditUser", "UserDetails", new { userId = userViewModel.Id });
            }
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
