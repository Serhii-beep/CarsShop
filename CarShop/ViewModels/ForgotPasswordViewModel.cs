﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CarShop.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Field must not be empty")]
        [EmailAddress]
        [Display(Name ="Email address")]
        public string Email { get; set; }
    }
}
