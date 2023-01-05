using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GcwensaHrApp.Enums
{
    public enum UserRole
    {
        [Display(Name = "Admin")]
        Admin = 1,

        [Display(Name = "Human Resources")]
        HrUser = 2,

        [Display(Name = "Employee")]
        Employee = 3
    }
}
