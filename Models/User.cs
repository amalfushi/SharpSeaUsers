using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserDashboard.Models
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public int UserLevel { get; set; }

        public IEnumerable<Conversation> Conversations { get; set; }

        public User() {
            Conversations = new List<Conversation>();
        }
    }

    public class LoginUser
    {
        [Required(ErrorMessage = "Email Address is required")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        // [RegularExpression()]
        public string LogEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string LogPassword { get; set; }
    }

    public class RegisterUser
    {
        private Regex reg;

        [Required(ErrorMessage = "First Name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        // [RegularExpression()]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password Confirmation is required")]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string C_Password { get; set; }

        public RegisterUser()
        {
            reg = new Regex(@"!\s{2,}");
        }
    }
}