using System;
using System.Collections.Generic;

namespace AirCinelMVC.Data.Entities.Dtos
{
    public class FlightDto
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public AirplaneDto Airplane { get; set; }
        public AirportDto DepartureAirport { get; set; }
        public AirportDto ArrivalAirport { get; set; }
        public List<TicketDto> Tickets { get; set; }
    }
}