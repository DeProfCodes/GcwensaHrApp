using GcwensaHrApp.Data;
using GcwensaHrApp.Enums;
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
                LeaveStatus = LeaveStatus.Pending
            };

            _dbContext.Add(leaveRequest);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditLeaveRequest(LeaveRequestViewModel leaveRequestVM)
        {
            var leaveReq = await GetLeaveRequestById(leaveRequestVM.LeaveId);

            leaveReq.LeaveStartDate = leaveRequestVM.LeaveStartDate;
            leaveReq.LeaveEndDate = leaveRequestVM.LeaveEndDate;
            leaveReq.LeaveDaysDuration = (int)leaveRequestVM.LeaveEndDate.Subtract(leaveRequestVM.LeaveStartDate).TotalDays;
            leaveReq.LeaveReason = leaveRequestVM.LeaveReason;
            leaveReq.LeaveType = leaveRequestVM.LeaveType;
            leaveReq.LastUpdateTime = DateTime.Now;

            _dbContext.Update(leaveReq);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CancelLeaveRequest(int leaveId)
        {
            var leaveReq = await GetLeaveRequestById(leaveId);

            leaveReq.LeaveStatus = LeaveStatus.Cancelled;

            _dbContext.Update(leaveReq);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteLeaveRequest(int leaveId)
        {
            var leaveReq = await GetLeaveRequestById(leaveId);

            leaveReq.LeaveStatus = LeaveStatus.Deleted;

            _dbContext.Update(leaveReq);
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

        public async Task<LeaveRequest> GetLeaveRequestById(int leaveId)
        {
            var leaveRequests = await _dbContext.LeaveRequests.FirstOrDefaultAsync(x => x.Id == leaveId);

            return leaveRequests;
        }

        public async Task CreateLeaveAvailable(string userId, double days)
        {
            var existing = await _dbContext.LeavesAvailable.FirstOrDefaultAsync(x=>x.UserId == userId);
            if (existing == null)
            {
                var available = new LeaveAvailable
                {
                    UserId = userId,
                    LeaveDaysAvailable = days
                };
                _dbContext.Add(available);
            }
            else
            {
                existing.LeaveDaysAvailable = days;
                _dbContext.Update(existing);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditLeaveAvailable(string userId, double days)
        {
            var existing = await _dbContext.LeavesAvailable.FirstOrDefaultAsync(x => x.UserId == userId);
            if (existing != null)
            {
                existing.LeaveDaysAvailable = days;
                _dbContext.Update(existing);
                
            }
            else
            {
                var available = new LeaveAvailable
                {
                    UserId = userId,
                    LeaveDaysAvailable = days
                };
                _dbContext.Add(available);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<LeaveAvailable>> GetAllLeavesAvailable()
        {
            var leavesDays = await _dbContext.LeavesAvailable.ToListAsync();

            return leavesDays;
        }

        public async Task<LeaveAvailable> GetUserLeaveAvailable(string userId)
        {
            var userLeaveDays = await _dbContext.LeavesAvailable.FirstOrDefaultAsync(x => x.UserId == userId);

            return userLeaveDays;
        }

        public async Task DeleteLeaveAvailable(string userId)
        {
            var userLeaveDays = await _dbContext.LeavesAvailable.FirstOrDefaultAsync(x => x.UserId == userId);
            if (userLeaveDays != null)
            {
                _dbContext.Remove(userLeaveDays);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
