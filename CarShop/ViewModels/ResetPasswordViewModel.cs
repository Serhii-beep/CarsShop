using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarShop.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Field must not be empty")]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field must not be empty")]
        [StringLength(100, ErrorMessage = "Password must contain more than 5 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Field must not be empty")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords are not same")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
