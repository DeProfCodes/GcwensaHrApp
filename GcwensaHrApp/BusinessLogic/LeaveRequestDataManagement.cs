using GcwensaHrApp.Data;
using GcwensaHrApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GcwensaHrApp.BusinessLogic
{
    public class LeaveRequestDataManagement : ILeaveRequestDataManagement
    {
        private readonly ApplicationDbContext dbContext;

        public LeaveRequestDataManagement(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task CreateLeaveRequest()
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<LeaveRequest>> GetAllLeaveRequests()
        {
            var allLeaveRequests = await dbContext.LeaveRequests.ToListAsync();

            return allLeaveRequests;    
        }
    }
}
