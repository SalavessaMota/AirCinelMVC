using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Data.Entities
{
    public class Country : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(4, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Code { get; set; }

        public ICollection<City> Cities { get; set; }


        [Display(Name = "Number of cities")]
        public int NumberCities => Cities == null ? 0 : Cities.Count;
    }
}
