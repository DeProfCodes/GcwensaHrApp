using GcwensaHrApp.Enums;
using GcwensaHrApp.Models;
using GcwensaHrApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GcwensaHrApp.BusinessLogic
{
    public interface IUserDataManagement
    {
        public Task<ApplicationUser> GetUserByEmail(string email);

        public Task<ApplicationUser> GetUserByUserId(string userId);

        public Task<string> GetUserRole(string userId);

        public Task CreateUser(UserDetailsViewModel userViewModel);

        public Task EditUser(UserDetailsViewModel userViewModel);

        public Task UpdateUserAccountStatus(string userId, AccountStatus accountStatus);

        public Task SuperDeleteUser(string userId);

        public Task<List<UserDetailsViewModel>> GetAllUsersDetails();
    }
}
