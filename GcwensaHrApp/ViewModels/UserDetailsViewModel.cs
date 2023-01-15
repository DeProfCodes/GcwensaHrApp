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

        [Display(Name = "Role")]
        public string UserRole { get; set; }

        [Display(Name = "Leave Days Available")]
        public double LeaveDaysAvailable { get; set; }

        [Display(Name = "Reset Password")]
        public bool ResetPassword { get; set; }

        [Display(Name = "Account Status")]
        public AccountStatus AccountStatus { get; set; }

        public string Password { get; set; }
    }
}
