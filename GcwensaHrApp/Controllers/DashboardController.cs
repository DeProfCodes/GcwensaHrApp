using GcwensaHrApp.BusinessLogic;
using GcwensaHrApp.Enums;
using GcwensaHrApp.Models;
using GcwensaHrApp.Services;
using GcwensaHrApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GcwensaHrApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserDataManagement _usersIO;
        private readonly ILeaveRequestDataManagement _leaveRequestIO;

        public DashboardController(UserManager<ApplicationUser> userManager, ILogger<DashboardController> logger, RoleManager<ApplicationRole> roleManager, IUserDataManagement usersIO, ILeaveRequestDataManagement leaveRequestIO)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;

            _usersIO = usersIO;
            _leaveRequestIO = leaveRequestIO;
            //_userManager.AddToRoleAsync(user, "admin");
        }

        private string GetLoggedInEmail()
        {
            return User.Identity.Name;
        }

        public async Task<ApplicationUser> GetLoggedInUser()
        {
            var user = await _usersIO.GetUserByEmail(User.Identity.Name);

            return user;
        }

        //[Authorize(Roles = "Admin,User")]

        public async Task<IActionResult> Index()
        {
            var user = await GetLoggedInUser();

            var leaveRequests = new List<LeaveRequest>();
            var userRole = await _usersIO.GetUserRole(user.Id);

            var isAdminOrHR = userRole == UserRole.Admin.GetDisplayName() || userRole == UserRole.HrUser.GetDisplayName();

            var avaialbleLeave = await _leaveRequestIO.GetUserLeaveAvailable(user.Id); 

            if (isAdminOrHR)
            {
                leaveRequests = await _leaveRequestIO.GetAllLeaveRequests();
            }
            else
            {
                leaveRequests = await _leaveRequestIO.GetAllUserLeaveRequests(user.Id);
            }

            var leaveRequestsVm = new List<LeaveRequestViewModel>();
            var allUsers = await _usersIO.GetAllUsers();

            foreach (var leaveReq in leaveRequests)
            {
                var leaveUser = allUsers.FirstOrDefault(x => x.Id == leaveReq.UserId);

                var leaveReqVm = new LeaveRequestViewModel
                {
                    LeaveId = leaveReq.Id,
                    LeaveRequestOwner = $"{leaveUser.Firstname} {leaveUser.Lastname}",
                    CreateTime = leaveReq.CreateTime,
                    LeaveType = leaveReq.LeaveType, 
                    LeaveStartDate = leaveReq.LeaveStartDate,
                    LeaveEndDate = leaveReq.LeaveEndDate,
                    LeaveDaysDuration = leaveReq.LeaveDaysDuration,
                    LeaveReason = leaveReq.LeaveReason,
                    LeaveStatus = leaveReq.LeaveStatus
                };
                leaveRequestsVm.Add(leaveReqVm);
            }
            
            var userVM = new UserDetailsViewModel
            {
                Firstname = user?.Firstname ?? "Error",
                Lastname = user?.Lastname ?? "Error",
                Email = user?.Email ?? "Error",
                LeaveDaysAvailable = avaialbleLeave?.LeaveDaysAvailable ?? 0.0
            };

            var dashboardVm = new DashboardViewModel()
            {
                LeaveRequests = leaveRequestsVm,
                UserDetails = userVM
            };

            return View(dashboardVm);
        }



        #region ADMINS USERS
        public async Task<IActionResult> UsersManagement()
        {
            var user = await GetLoggedInUser();
            var userRole = await _usersIO.GetUserRole(user.Id);
            var authorized = (userRole == UserRole.Admin.GetDisplayName() || userRole == UserRole.HrUser.GetDisplayName());
            if (authorized)
            {
                return RedirectToAction("UsersManagement", "UserDetails");
            }
            else
            {
                return Unauthorized();
            }
        }

        public async Task<IActionResult> LeavesManagement()
        {
            var user = await GetLoggedInUser();
            var authorized = await _userManager.IsInRoleAsync(user, UserRole.Admin.GetDisplayName());
            if (authorized)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return Unauthorized();
            }
        }

        public async Task<IActionResult> SendGrid()
        {
            var user = await GetLoggedInUser();

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                                        "/Account/ConfirmEmail",
                                        pageHandler: null,
                                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = "" },
                                        protocol: Request.Scheme
                                      );

            //await _emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            // await Execute("Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.", "Profi384@gmail.com");
            await Test();
            return RedirectToAction("Index", "Dashboard");
        }

        public async Task Test()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("Proficient@gcwensa.co.za", "Example User");
            var subject = "Sending with Twilio SendGrid is Fun";
            var to = new EmailAddress("profi384@gmail.com", "Example User");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            int t = 5;
        }

        public async Task Execute(string subject, string message, string toEmail)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Proficient@gcwensa.co.za", "Email Activation"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }
        #endregion
    }
}
