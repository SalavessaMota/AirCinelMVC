using AirCinelMVC.Data.Entities;
using System.Linq;

namespace AirCinelMVC.Data
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        public IQueryable<Flight> GetAllFlightsWithAirplaneAndAirports();

        public bool ExistFlight(int id);
    }
}
