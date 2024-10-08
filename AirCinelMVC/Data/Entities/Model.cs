using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Data.Entities
{
    public class Model : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characters.")]
        public string Name { get; set; }

        public ICollection<Airplane> Airplanes { get; set; }

        [Display(Name = "Number of Airplanes")]
        public int NumberOfAirplanes => Airplanes == null ? 0 : Airplanes.Count;
    }
}