using AirCinelMVC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirCinelMVC.Data
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        private readonly DataContext _context;

        public FlightRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Flight> GetAllFlightsWithAirplaneAndAirports()
        {
            return _context.Flights
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport);
        }

        public async Task<Flight> GetFlightWithAirplaneAndAirports(int id)
        {
            return await _context.Flights
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public IQueryable GetAllFlightsWithTickets()
        {
            return _context.Flights
                .Include(f => f.Tickets)
                .ThenInclude(t => t.User);
        }

        public IEnumerable<Flight> GetFlightsByUserId(string userId)
        {
            return _context.Flights
                .Include(f => f.Tickets)
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .Where(f => f.Tickets.Any(t => t.UserId == userId))
                .ToList();
        }

        public async Task<Flight> GetFlightWithAirplaneAirportsAndTickets(int id)
        {
            return await _context.Flights
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.Tickets)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public bool ExistFlight(int id)
        {
            return _context.Flights.Any(f => f.Id == id);
        }


        public async Task<Ticket> GetTicketWithUserFlightAirplaneAndAirports(int id)
        {
            return await _context.Tickets
                .Include(t => t.Flight)
                .ThenInclude(f => f.Airplane)
                .Include(t => t.Flight.DepartureAirport)
                .Include(t => t.Flight.ArrivalAirport)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }


        public async Task<IEnumerable<Ticket>> GetTicketsByUserIdAsync(string userId)
        {
            return await _context.Tickets
                                 .Include(t => t.Flight)
                                 .Include(t => t.Flight.DepartureAirport)
                                 .Include(t => t.Flight.ArrivalAirport)
                                 .Where(t => t.UserId == userId)
                                 .ToListAsync();
        }

    }
}
