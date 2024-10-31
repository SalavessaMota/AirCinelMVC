using System;

namespace AirCinelMVC.Data.Dtos
{
    public class UpdateFlightDateDto
    {
        public int Id { get; set; }
        public DateTime NewStart { get; set; }
        public DateTime NewEnd { get; set; }
    }
}
