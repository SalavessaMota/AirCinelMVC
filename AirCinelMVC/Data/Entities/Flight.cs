using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Data.Entities
{
    public class Flight : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Airplane")]
        public int AirplaneID { get; set; }
        public Airplane Airplane { get; set; }


        [Required]
        [Display(Name = "Departure Airport")]
        public int DepartureAirportID { get; set; }
        public Airport DepartureAirport { get; set; }


        [Required]
        [Display(Name = "Arrival Airport")]
        public int ArrivalAirportID { get; set; }
        public Airport ArrivalAirport { get; set; }


        [Required]
        [Display(Name = "Departure Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime DepartureTime { get; set; }


        [Required]
        [Display(Name = "Arrival Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime ArrivalTime { get; set; }


        [Display(Name = "Flight Number")]
        public string FlightNumber
        {
            get
            {
                return $"AC{Id:D4}"; // Exemplo: "AC0001", "AC0010", etc.
            }
        }



        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();


    }
}
