using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "The password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "The password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
            ErrorMessage = "The password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string Confirm { get; set; }
    }
}