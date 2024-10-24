namespace AirCinelMVC.Data.Dtos
{
    public class BoughtTicketDto
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; }
        public FlightDto Flight { get; set; }
    }
}
