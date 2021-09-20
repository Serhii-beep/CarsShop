using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarShop.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Field must not be empty")]
        [Display(Name = "Email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Field must not be empty")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Field must not be empty")]
        [Compare("Password", ErrorMessage = "Passwords are not same")]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
