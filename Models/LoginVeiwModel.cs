using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CustomAuth.Models
{
    public class LoginVeiwModel
    {
        [Required(ErrorMessage = "Username or Email is required")]
        [MaxLength(100, ErrorMessage = "Max 100 Characters allowed. ")]
        [DisplayName("Username Or Email")]
        public string UsernameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Max 20 or min 5 Characters allowed. ")]
        public string Password { get; set; }
    }
}
