using GcwensaHrApp.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GcwensaHrApp.ViewModels
{
    public class LeaveRequestViewModel
    {
        public string LeaveRequestOwner { get; set; }

        public int LeaveId { get; set; }
        
        [Required]  
        [DataType(DataType.Date)]
        public DateTime LeaveStartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime LeaveEndDate { get; set; }

        [Required]  
        public LeaveType LeaveType { get; set; }

        [Required]  
        public string LeaveReason { get; set; }

        public DateTime CreateTime { get; set; }

        public LeaveStatus LeaveStatus { get; set; }

        public int LeaveDaysDuration { get; set; }
    }
}
