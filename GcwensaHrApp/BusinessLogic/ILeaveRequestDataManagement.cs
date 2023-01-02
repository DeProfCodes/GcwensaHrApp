using GcwensaHrApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GcwensaHrApp.BusinessLogic
{
    public interface ILeaveRequestDataManagement
    {

        public Task<List<LeaveRequest>> GetAllLeaveRequests();

        public Task CreateLeaveRequest();


    }
}
