using GcwensaHrApp.Models;
using System.Collections.Generic;

namespace GcwensaHrApp.ViewModels
{
    public class DashboardViewModel
    {
        public List<LeaveRequest> LeaveRequests { get; set; }

        public UserDetailsViewModel UserDetails { get; set; }
    }
}
