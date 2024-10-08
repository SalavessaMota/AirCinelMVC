using System;
using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Data.Entities
{
    public class Airplane : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characters.")]
        public string Model { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characters.")]
        public string Manufacturer { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        public int Capacity { get; set; }

        [Display(Name = "Year of Manufacture")]
        public int YearOfManufacture { get; set; }

        public int ModelId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://aircinelmvc.blob.core.windows.net/resources/noimage.png"
            : $"https://aircinelmvc.blob.core.windows.net/airplanes/{ImageId}";
    }
}