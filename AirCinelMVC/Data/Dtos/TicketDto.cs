﻿namespace AirCinelMVC.Data.Dtos
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string SeatNumber { get; set; }
        public UserDto User { get; set; }
    }
}