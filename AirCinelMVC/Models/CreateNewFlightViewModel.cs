using AirCinelMVC.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Models
{
    public class CreateNewFlightViewModel : Flight
    {
        [Display(Name = "Departure City")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a city.")]
        public int CityId { get; set; }

        public IEnumerable<SelectListItem> Cities { get; set; }

        [Display(Name = "Departure Country")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a country.")]
        public int CountryId { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }

        [Display(Name = "Departure Airport")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a departure airport.")]
        public int DepartureAirportId { get; set; }

        public IEnumerable<SelectListItem> DepartureAirports { get; set; }

        [Display(Name = "Arrival Airport")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select an arrival airport.")]
        public int ArrivalAirportId { get; set; }

        public IEnumerable<SelectListItem> ArrivalAirports { get; set; }
    }
}