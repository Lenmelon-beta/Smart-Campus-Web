using System.ComponentModel.DataAnnotations;

namespace CustomAuth.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "Max 50 Characters allowed. ")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Max 50 Characters allowed. ")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [MaxLength(100, ErrorMessage = "Max 100 Characters allowed. ")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Username is required")]
        [MaxLength(20, ErrorMessage = "Max 20 Characters allowed. ")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Max 20 or min 5 Characters allowed. ")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Please comfirm your password")]
        [DataType(DataType.Password)]
        [MaxLength(20, ErrorMessage = "Max 20 Characters allowed. ")]
        public string ConfirmPassword { get; set; }


    }
}
