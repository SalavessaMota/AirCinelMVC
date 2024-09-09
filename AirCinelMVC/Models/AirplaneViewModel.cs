using AirCinelMVC.Data.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Models
{
    public class AirplaneViewModel : Airplane
    {
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }
    }
}
