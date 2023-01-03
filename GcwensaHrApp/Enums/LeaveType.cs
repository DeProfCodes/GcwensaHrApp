using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GcwensaHrApp.Enums
{
    public enum LeaveType
    {
        [Display(Name = "Sick")]
        Sick = 1,

        [Display(Name = "Maternity")]
        Maternity = 2,

        [Display(Name = "Family Responsibility")]
        FamilyResponsibility = 3,

        [Display(Name = "Other")]
        Other = 4,
    }
}
