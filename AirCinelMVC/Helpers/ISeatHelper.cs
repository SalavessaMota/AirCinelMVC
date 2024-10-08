using AirCinelMVC.Data.Entities;
using System.Collections.Generic;

namespace AirCinelMVC.Helpers
{
    public interface ISeatHelper
    {
        public List<int> GetAvailableSeats(Flight flight);

        public string ConvertSeatNumber(int seatNumber, string airplaneModel);

        public int ConvertSeatStringToNumber(string seatString, string airplaneModel);

        public int GetSeatsAndCorridorsPerRowByModel(string model);

        public int[,] GenerateSeatMap(string airplaneModel, int capacity);

        public List<int> GetCorridorPositions(int seatsPerRow);
    }
}
