using GcwensaHrApp.Data;
using GcwensaHrApp.Models;
using GcwensaHrApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace GcwensaHrApp.BusinessLogic
{
    public class LeaveRequestDataManagement : ILeaveRequestDataManagement
    {
        private readonly ApplicationDbContext _dbContext;

        public LeaveRequestDataManagement(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateLeaveRequest(string userId, LeaveRequestViewModel leaveRequestVM)
        {
            var leaveRequest = new LeaveRequest
            {
                UserId = userId,
                CreateTime = DateTime.Now,
                LastUpdateTime = DateTime.Now,
                LeaveDaysDuration = (int) leaveRequestVM.LeaveEndDate.Subtract(leaveRequestVM.LeaveStartDate).TotalDays,
                LeaveStartDate = leaveRequestVM.LeaveStartDate,
                LeaveEndDate = leaveRequestVM.LeaveEndDate,
                LeaveReason = leaveRequestVM.LeaveReason,    
                LeaveType = leaveRequestVM.LeaveType,    
            };

            _dbContext.Add(leaveRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<LeaveRequest>> GetAllLeaveRequests()
        {
            var allLeaveRequests = await _dbContext.LeaveRequests.ToListAsync();

            return allLeaveRequests;    
        }

        public async Task<List<LeaveRequest>> GetAllUserLeaveRequests(string userId)
        {
            var leaveRequests = await _dbContext.LeaveRequests.Where(x => x.UserId == userId).ToListAsync();

            return leaveRequests;
        }
    }
}
