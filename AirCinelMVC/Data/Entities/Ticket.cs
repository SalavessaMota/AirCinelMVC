using System.ComponentModel.DataAnnotations;

namespace AirCinelMVC.Data.Entities
{
    public class Ticket : IEntity
    {
        public int Id { get; set; }

        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        [MaxLength(10)]
        public string SeatNumber { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}