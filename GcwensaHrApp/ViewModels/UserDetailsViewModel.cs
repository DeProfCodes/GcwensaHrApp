using GcwensaHrApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace GcwensaHrApp.ViewModels
{
    public class UserDetailsViewModel
    {
        public string Id { get; set; }
        
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string UserRole { get; set; }

        public double LeaveDaysAvailable { get; set; }

        public AccountStatus AccountStatus { get; set; }
    }
}
