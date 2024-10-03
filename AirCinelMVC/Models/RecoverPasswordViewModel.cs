using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
