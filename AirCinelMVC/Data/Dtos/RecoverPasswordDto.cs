using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Data.Dtos
{
    public class RecoverPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}