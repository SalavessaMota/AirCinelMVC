using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [EmailAddress]
        public string Username { get; set; }


        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MinLength(6, ErrorMessage = "The field {0} must contain at least {1} characters.")]
        public string Password { get; set; }


        public bool RememberMe { get; set; }
    }
}
