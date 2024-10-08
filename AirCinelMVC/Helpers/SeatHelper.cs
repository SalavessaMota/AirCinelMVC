using AirCinelMVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirCinelMVC.Helpers
{
    public class SeatHelper : ISeatHelper
    {
        public List<int> GetAvailableSeats(Flight flight)
        {
            var reservedSeats = flight.Tickets
                                    .Select(t => ConvertSeatStringToNumber(t.SeatNumber, flight.Airplane.Model))
                                    .ToList();

            var totalSeats = Enumerable.Range(1, flight.Airplane.Capacity).ToList();
            return totalSeats.Except(reservedSeats).ToList();
        }

        public string ConvertSeatNumber(int seatNumber, string airplaneModel)
        {
            int seatsPerRow = GetSeatsPerRowByModel(airplaneModel);
            int row = (seatNumber - 1) / seatsPerRow + 1;
            int seatPositionInRow = (seatNumber - 1) % seatsPerRow;
            char seatLetter = (char)('A' + seatPositionInRow);
            return $"{row}{seatLetter}";
        }

        public int ConvertSeatStringToNumber(string seatString, string airplaneModel)
        {
            string rowPart = new string(seatString.TakeWhile(char.IsDigit).ToArray());
            char seatLetter = seatString.Last();

            int row = int.Parse(rowPart);
            int seatsPerRow = GetSeatsPerRowByModel(airplaneModel);
            int seatPositionInRow = seatLetter - 'A';

            return (row - 1) * seatsPerRow + seatPositionInRow + 1;
        }

        public int GetSeatsPerRowByModel(string model)
        {
            return model switch
            {
                "A319" => 6,
                "A320" => 6,
                "A330" => 8,
                "A350" => 9,
                "A380" => 10,
                "737" => 6,
                "747" => 10,
                "757" => 6,
                "767" => 7,
                "777" => 9,
                "E170" => 4,
                "E175" => 4,
                "E190" => 4,
                "E195" => 4,
                _ => 6
            };
        }
    }
}
