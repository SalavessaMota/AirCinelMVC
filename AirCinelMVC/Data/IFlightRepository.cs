using AirCinelMVC.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace AirCinelMVC.Data
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        public IQueryable<Flight> GetAllFlightsWithAirplaneAndAirports();

        public Task<Flight> GetFlightWithAirplaneAndAirports(int id);

        public bool ExistFlight(int id);
    }
}
