using GcwensaHrApp.Models;
using System.Collections.Generic;

namespace GcwensaHrApp.ViewModels
{
    public class DashboardViewModel
    {
        public List<LeaveRequestViewModel> LeaveRequests { get; set; }

        public UserDetailsViewModel UserDetails { get; set; }
    }
}
