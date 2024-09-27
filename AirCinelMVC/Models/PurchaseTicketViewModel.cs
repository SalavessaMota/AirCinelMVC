using System.Collections.Generic;

namespace AirCinelMVC.Models
{
    public class PurchaseTicketViewModel
    {
        public int FlightId { get; set; }
        public List<string> AvailableSeats { get; set; } = new List<string>();
        public string SelectedSeat { get; set; }
    }
}
