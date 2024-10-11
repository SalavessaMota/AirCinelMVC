using System.Collections.Generic;

namespace AirCinelMVC.Models
{
    public class PurchaseTicketViewModel
    {
        public int FlightId { get; set; }
        public int[,] SeatMap { get; set; }
        public string SelectedSeat { get; set; }
        public List<string> OccupiedSeats { get; set; }
    }
}