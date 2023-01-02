using GcwensaHrApp.Models;
using System.Threading.Tasks;

namespace GcwensaHrApp.BusinessLogic
{
    public interface IUserDataManagement
    {
        public Task<ApplicationUser> GetUserByEmail(string email);
    }
}
