using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GcwensaHrApp.Enums
{
    public enum LeaveStatus
    {
        [Display(Name = "Pending")]
        Pending = 1,

        [Display(Name = "In Progress")]
        InProgress = 2,

        [Display(Name = "Approved")]
        Approved = 3,

        [Display(Name = "Rejected")]
        Rejected = 4,

        [Display(Name = "Deleted")]
        Deleted = 5,

        [Display(Name = "Cancelled")]
        Cancelled = 6
    }
}
