using GcwensaHrApp.Models;
using GcwensaHrApp.ViewModels;
using System.Runtime.InteropServices;

namespace GcwensaHrApp.Helpers
{
    public class ViewModelHelper
    {
        public static UserDetailsViewModel CreateUserViewModel(ApplicationUser user, string role)
        {
            var userVM = new UserDetailsViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Firstname = user.Firstname, 
                Lastname = user.Lastname,
                PhoneNumber = user.PhoneNumber,
                UserRole = role,
                AccountStatus = user.AccountStatus
            };

            return userVM;  
        }
    }
}
