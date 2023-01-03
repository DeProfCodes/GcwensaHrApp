using GcwensaHrApp.Models;
using GcwensaHrApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GcwensaHrApp.BusinessLogic
{
    public interface ILeaveRequestDataManagement
    {

        public Task<List<LeaveRequest>> GetAllLeaveRequests();

        public Task<List<LeaveRequest>> GetAllUserLeaveRequests(string userId);

        public Task CreateLeaveRequest(string userId, LeaveRequestViewModel leaveRequestVM);


    }
}
