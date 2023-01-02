using GcwensaHrApp.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GcwensaHrApp.Controllers
{
    public class LeaveRequestsController : Controller
    {
        private readonly ILeaveRequestDataManagement leaveRequestIO;

        public LeaveRequestsController(ILeaveRequestDataManagement leaveRequestIO)
        {
            this.leaveRequestIO = leaveRequestIO;   
        }

        public async Task<IActionResult> Index()
        {
            var allLeaveRequests = await leaveRequestIO.GetAllLeaveRequests();

            return View(allLeaveRequests);
        }

        #region Employee Leaves Management
        public IActionResult EmployeeLeaveRequests()
        {
            return View();    
        }

        public IActionResult CreateLeaveRequest()
        {

            return View();  
        }

        public IActionResult EditLeaveRequest(int leaveId)
        {
            return View();  
        }

        public IActionResult DeleteLeaveRequest(int leaveId)
        {
            return View();
        }
        #endregion

        #region HR Leaves Management
        public IActionResult AllLeaveRequests()
        {
            return View();
        }

        public IActionResult ManageLeaveRequest(int leaveId)
        {
            return View();
        }

        #endregion


    }
}
