using AirCinelMVC.Data.Entities;
using System.Collections.Generic;

namespace AirCinelMVC.Helpers
{
    public interface ISeatHelper
    {
        public List<int> GetAvailableSeats(Flight flight);

        public string ConvertSeatNumber(int seatNumber, string airplaneModel);

        public int ConvertSeatStringToNumber(string seatString, string airplaneModel);

        public int GetSeatsPerRowByModel(string model);

    }
}
