using System;

namespace GcwensaHrApp.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int LeaveDaysDuration { get; set; }

        public DateTime LeaveStartDate { get; set; }

        public DateTime LeaveEndDate { get; set; }

        public string LeaveReason { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime LastUpdateTime { get; set; }
    }
}
