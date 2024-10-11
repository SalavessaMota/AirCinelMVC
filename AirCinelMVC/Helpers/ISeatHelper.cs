using AirCinelMVC.Data.Entities;
using System.Collections.Generic;

namespace AirCinelMVC.Helpers
{
    public interface ISeatHelper
    {
        List<int> GetAvailableSeats(Flight flight);

        string ConvertSeatNumber(int seatNumber, string airplaneModel);

        int ConvertSeatStringToNumber(string seatString, string airplaneModel);

        int GetSeatsAndCorridorsPerRowByModel(string model);

        int[,] GenerateSeatMap(string airplaneModel, int capacity);

        List<int> GetCorridorPositions(int seatsPerRow);
    }
}