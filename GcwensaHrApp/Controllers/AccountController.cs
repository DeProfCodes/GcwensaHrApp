using GcwensaHrApp.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using GcwensaHrApp.Models;
using GcwensaHrApp.Enums;
using GcwensaHrApp.BusinessLogic;
using GcwensaHrApp.Helpers;
using System;
using Microsoft.EntityFrameworkCore.Storage;
using GcwensaHrApp.Data;
using GcwensaHrApp.ViewModels;

namespace GcwensaHrApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserDataManagement _usersIO;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _dbContext;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger, IEmailSender emailSender, IUserDataManagement usersIO, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _usersIO = usersIO;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var returnUrl = Url.Content("~/");
                var user = new ApplicationUser
                {
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email,//model.Username, 
                    Email = model.Email,
                    AccountStatus = AccountStatus.Active
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.RemoveFromRoleAsync(user, UserRole.Employee.GetDisplayName());
                    await _userManager.AddToRoleAsync(user, UserRole.Employee.GetDisplayName());

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                                                "/Account/ConfirmEmail",
                                                pageHandler: null,
                                                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                                                protocol: Request.Scheme
                                              );

                    await _emailSender.SendEmailAsync(model.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        //return RedirectToPage("RegisterConfirmation", new { email = model.Email, returnUrl = returnUrl });
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("Index", "Dashboard");
                }
                if (result.RequiresTwoFactor)
                {
                    //return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        public async Task<IActionResult> AccountSettings()
        {
            var user = await _usersIO.GetUserByEmail(User.Identity.Name);
            var userDtailsVm = ViewModelHelper.CreateUserViewModel(user, "");
            
            return View(userDtailsVm);
        }

        [HttpPost]
        public async Task<IActionResult> AccountSettings(UserDetailsViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                userViewModel.Email = User.Identity.Name;
                if (!string.IsNullOrEmpty(userViewModel.Password))
                {
                    if (!Validators.IsValidPassword(userViewModel.Password))
                    {
                        ModelState.AddModelError(string.Empty, "Password must 8 or more characters, upper case, lowecase, number, special characters!");
                        return View(userViewModel);
                    }
                }

                IDbContextTransaction transaction = _dbContext.Database.BeginTransaction();
                try
                {
                    await _usersIO.EditUser(userViewModel);

                    transaction.Commit();

                    return RedirectToAction("Index", "Dashboard");
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    ModelState.AddModelError(string.Empty, e.Message);
                    return View(userViewModel);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error.");
                return View(userViewModel);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account"); 
        }
    }
}
