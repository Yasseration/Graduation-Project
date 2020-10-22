using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GraduationProject.Models
{
    [MetadataType(typeof(PatientMetadata))]
    public partial class Patient
    {
    }

    public class PatientMetadata
    {
        [MinLength(2,ErrorMessage = "First Name at least 2 charcters .")]
        [MaxLength(50,ErrorMessage = "First Name max 50 charcters .")]
        [Required(ErrorMessage = "Please Enter First Name .")]
        public string FName { get; set; }

        [MinLength(2, ErrorMessage = "Last Name at least 2 charcters .")]
        [MaxLength(50, ErrorMessage = "Last Name max 50 charcters .")]
        [Required(ErrorMessage = "Please Enter Last Name .")]
        public string LName { get; set; }

        [Range(1, 120, ErrorMessage = "Range must be between 1:120 years .")]
        [Required(ErrorMessage = "Please Enter Age .")]
        public short Age { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please Enter Your Email Address .")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 charcters .")]
        [MaxLength(50, ErrorMessage = "Password must be 50 charcters max .")]
        [Required(ErrorMessage = "Please Enter Your Password .")]
        public string PW { get; set; }

        [DataType(DataType.Upload)]
        public byte[] Img { get; set; }
    }
}