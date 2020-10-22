using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GraduationProject.Models.Extended
{
    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please Enter Your Email Address .")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 charcters .")]
        [MaxLength(50, ErrorMessage = "Password must be 50 charcters max .")]
        [Required(ErrorMessage = "Please Enter Your Password .")]
        public string PW { get; set; }
    }
}