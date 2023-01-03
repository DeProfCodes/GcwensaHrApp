using GcwensaHrApp.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace GcwensaHrApp.ViewModels
{
    public class LeaveRequestViewModel
    {

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
    }
}
