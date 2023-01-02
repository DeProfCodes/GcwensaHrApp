using GcwensaHrApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GcwensaHrApp.BusinessLogic
{
    public class UserDataManagement : IUserDataManagement
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserDataManagement(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }
    }
}
