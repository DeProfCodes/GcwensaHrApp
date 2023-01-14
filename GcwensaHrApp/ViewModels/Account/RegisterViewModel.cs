using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace GcwensaHrApp.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Username can not be left blank.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Firstname can not be left blank.", MinimumLength = 1)]
        public string Firstname { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Lastname can not be left blank.", MinimumLength = 1)]
        public string Lastname { get; set; }

        [Required]
        [Phone]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid cellphone number")]
        [StringLength(10, ErrorMessage = "Phone number too short", MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [StringLength(50, ErrorMessage = "Email can not be left blank.", MinimumLength = 1)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
