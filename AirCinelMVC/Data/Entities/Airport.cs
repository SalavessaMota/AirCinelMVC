using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AirCinelMVC.Data.Entities
{
    public class Airport : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characters.")]
        public string Name { get; set; }

        public int CityId { get; set; }

        [JsonIgnore]
        public City City { get; set; }

        [Display(Name = "Country Flag")]
        public Guid ImageId { get; set; }

        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://aircinelmvc.blob.core.windows.net/resources/noimage.png"
            : $"https://aircinelmvc.blob.core.windows.net/flags/{ImageId}";
    }
}