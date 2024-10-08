using AirCinelMVC.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirCinelMVC.Data
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        public IQueryable<Flight> GetAllFlightsWithAirplaneAndAirports();

        public Task<Flight> GetFlightWithAirplaneAndAirports(int id);

        public IQueryable GetAllFlightsWithTickets();

        public IEnumerable<Flight> GetFlightsByUserId(string userId);

        public Task<Flight> GetFlightWithAirplaneAirportsAndTickets(int id);

        public bool ExistFlight(int id);

        public Task<Ticket> GetTicketWithUserFlightAirplaneAndAirports(int id);

        public Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId);
    }
}