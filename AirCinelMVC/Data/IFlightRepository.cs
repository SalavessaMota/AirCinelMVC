using AirCinelMVC.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirCinelMVC.Data
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        IQueryable<Flight> GetAllFlights();

        IQueryable<Flight> GetAllFlightsWithAirplaneAndAirports();

        IQueryable<Flight> GetAllFlightsWithAirplaneAirportsAndTickets();

        Task<Flight> GetFlightWithAirplaneAndAirportsAsync(int id);

        IEnumerable<Flight> GetFlightsByUserId(string userId);

        Task<Flight> GetFlightWithAirplaneAirportsAndTicketsAsync(int id);

        Task<Ticket> GetTicketWithUserFlightAirplaneAndAirportsAsync(int id);

        Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId);

        Task<IEnumerable<Ticket>> GetAllTicketsWithAllInfoAsync();
    }
}