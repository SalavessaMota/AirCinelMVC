using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AirCinelMVC.Data.Entities
{
    public class City : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The field {0} is mandatory.")]
        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characters.")]
        public string Name { get; set; }

        public int CountryId { get; set; }

        [JsonIgnore]
        public Country Country { get; set; }

        public ICollection<Airport> Airports { get; set; } = new List<Airport>();

        [Display(Name = "Number of Airports")]
        public int NumberOfAirports => Airports == null ? 0 : Airports.Count;
    }
}