using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Data.Entities
{
    public class Flight : IEntity
    {
        public int Id { get; set; }


        public int AirplaneID { get; set; }
        public Airplane Airplane { get; set; }


        [Display(Name = "Departure Airport")]
        public int DepartureAirportID { get; set; }
        public Airport DepartureAirport { get; set; }


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


        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
