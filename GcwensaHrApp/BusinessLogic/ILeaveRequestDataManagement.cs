using GcwensaHrApp.Models;
using GcwensaHrApp.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GcwensaHrApp.BusinessLogic
{
    public interface ILeaveRequestDataManagement
    {

        public Task<List<LeaveRequest>> GetAllLeaveRequests();

        public Task<LeaveRequest> GetLeaveRequestById(int leaveId);

        public Task<List<LeaveRequest>> GetAllUserLeaveRequests(string userId);

        public Task CreateLeaveRequest(string userId, LeaveRequestViewModel leaveRequestVM);

        public Task EditLeaveRequest(LeaveRequestViewModel leaveRequestVM);

        public Task CancelLeaveRequest(int leaveId);

        public Task DeleteLeaveRequest(int leaveId);

        public Task CreateLeaveAvailable(string userId, double days);

        public Task EditLeaveAvailable(string userId, double days);

        public Task<List<LeaveAvailable>> GetAllLeavesAvailable();

        public Task<LeaveAvailable> GetUserLeaveAvailable(string userId);

        public Task DeleteLeaveAvailable(string userId);

    }
}
