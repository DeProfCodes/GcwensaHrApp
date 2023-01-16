using GcwensaHrApp.Enums;
using GcwensaHrApp.Models;
using GcwensaHrApp.ViewModels;
using GcwensaHrApp.ViewModels.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GcwensaHrApp.BusinessLogic
{
    public interface IUserDataManagement
    {
        public Task<ApplicationUser> GetUserByEmail(string email);

        public Task<ApplicationUser> GetUserByUserId(string userId);

        public Task<List<ApplicationUser>> GetAllUsers();

        public Task<string> GetUserRole(string userId);

        public Task<UserDetailsViewModel> CreateUser(UserDetailsViewModel userViewModel);

        public Task<string> EditUser(UserDetailsViewModel userViewModel);

        public Task UpdateUserAccountStatus(string userId, AccountStatus accountStatus);

        public Task SuperDeleteUser(string userId);

        public Task<List<UserDetailsViewModel>> GetAllUsersDetails();
    }
}
