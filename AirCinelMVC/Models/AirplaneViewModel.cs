using AirCinelMVC.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Models
{
    public class AirplaneViewModel : Airplane
    {
        [Required]
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }


        [Display(Name = "Model")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a model.")]
        public int ModelId { get; set; }
        public IEnumerable<SelectListItem> Models { get; set; }



        [Display(Name = "Manufacturer")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a manufacturer.")]
        public int ManufacturerId { get; set; }
        public IEnumerable<SelectListItem> Manufacturers { get; set; }

    }
}
