using GcwensaHrApp.Data;
using GcwensaHrApp.Enums;
using GcwensaHrApp.Models;
using GcwensaHrApp.ViewModels;
using GcwensaHrApp.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GcwensaHrApp.BusinessLogic
{
    public class UserDataManagement : IUserDataManagement
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserDataManagement(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<UserDetailsViewModel> CreateUser(UserDetailsViewModel userViewModel)
        {
            var user = new ApplicationUser
            {
                Firstname = userViewModel.Firstname,
                Lastname = userViewModel.Lastname,
                PhoneNumber = userViewModel.PhoneNumber,
                UserName = userViewModel.Email,
                Email = userViewModel.Email,
                AccountStatus = userViewModel.AccountStatus
            };

            _dbContext.Add(user);
            await _dbContext.SaveChangesAsync();

            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userViewModel.Email);

            var newPassword = "Default1!";
            await _userManager.AddPasswordAsync(dbUser, newPassword);

            await _userManager.AddToRoleAsync(dbUser, userViewModel.UserRole);

            userViewModel.Id = dbUser.Id;
            userViewModel.Password = newPassword;

            return userViewModel;
        }

        private async Task UpdateUserRole(ApplicationUser user, string newRole)
        {
            var userRole = await GetUserRole(user.Id);

            await _userManager.RemoveFromRoleAsync(user, userRole);
            await _userManager.AddToRoleAsync(user, newRole);
        }

        public async Task<string> EditUser(UserDetailsViewModel userViewModel)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == userViewModel.Email);

            dbUser.Firstname = userViewModel.Firstname;
            dbUser.Lastname = userViewModel.Lastname;
            dbUser.PhoneNumber = userViewModel.PhoneNumber;
            dbUser.UserName = userViewModel.Email;
            dbUser.Email = userViewModel.Email;
            dbUser.AccountStatus = userViewModel.AccountStatus;

            var newPassword = "Default1!";

            _dbContext.Update(dbUser);
            await _dbContext.SaveChangesAsync();

            await UpdateUserRole(dbUser, userViewModel.UserRole);

            return newPassword;
        }

        public async Task UpdateUserAccountStatus(string userId, AccountStatus accountStatus)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            dbUser.AccountStatus = accountStatus;

            _dbContext.Update(dbUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            
            return user;
        }

        public async Task<ApplicationUser> GetUserByUserId(string userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            return user;
        }

        public async Task<string> GetUserRole(string userId)
        {
            var allRoles = await _roleManager.Roles.ToListAsync();

            var userRoleId = (await _dbContext.ApplicationUserRoles.FirstOrDefaultAsync(x=> x.UserId == userId)).RoleId;
            var userRole = allRoles.FirstOrDefault(x => x.Id == userRoleId).Name;

            return userRole;
        }

        public async Task<List<UserDetailsViewModel>> GetAllUsersDetails()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var allRoles = await _roleManager.Roles.ToListAsync();
            var allUserRoles = await _dbContext.ApplicationUserRoles.ToListAsync();
            
            var usersViewModel = new List<UserDetailsViewModel>();
            
            foreach (var user in allUsers)
            {
                var userRoleId = allUserRoles.Where(c => c.UserId == user.Id).FirstOrDefault().RoleId;
                var userRole = allRoles.FirstOrDefault(x => x.Id == userRoleId).Name; 

                var usrVm = new UserDetailsViewModel
                {
                    Id = user.Id,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserRole = userRole,
                    AccountStatus = user.AccountStatus
                };
                usersViewModel.Add(usrVm);
            }

            return usersViewModel;
        }

        public async Task SuperDeleteUser(string userId)
        {
            var dbUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

            var userRole = await GetUserRole(userId);

            await _userManager.RemoveFromRoleAsync(dbUser, userRole);

            _dbContext.Remove(dbUser);
            await _dbContext.SaveChangesAsync();
        }
    }
}
