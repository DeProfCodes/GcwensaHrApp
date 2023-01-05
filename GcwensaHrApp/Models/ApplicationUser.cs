using GcwensaHrApp.Enums;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace GcwensaHrApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public AccountStatus AccountStatus { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
