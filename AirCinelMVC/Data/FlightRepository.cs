using AirCinelMVC.Data.Entities;
using Microsoft.EntityFrameworkCore;
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

        public IQueryable<Flight> GetAllFlights()
        {
            return _context.Flights;
        }

        public IQueryable<Flight> GetAllFlightsWithAirplaneAirportsAndTickets()
        {
            return _context.Flights
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.Tickets);
        }

        public IQueryable<Flight> GetAllFlightsWithAirplaneAndAirports()
        {
            return _context.Flights
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport);
        }

        public async Task<Flight> GetFlightWithAirplaneAndAirportsAsync(int id)
        {
            return await _context.Flights
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .FirstOrDefaultAsync(f => f.Id == id);
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

        public async Task<Flight> GetFlightWithAirplaneAirportsAndTicketsAsync(int id)
        {
            return await _context.Flights
                .Include(f => f.Airplane)
                .Include(f => f.DepartureAirport)
                .Include(f => f.ArrivalAirport)
                .Include(f => f.Tickets)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Ticket> GetTicketWithUserFlightAirplaneAndAirportsAsync(int id)
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
                                 .Include(t => t.Flight.Airplane)
                                 .Where(t => t.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsWithAllInfoAsync()
        {
            return await _context.Tickets
             .Include(t => t.Flight)
             .ThenInclude(f => f.Airplane)
             .Include(t => t.Flight.DepartureAirport)
             .Include(t => t.Flight.ArrivalAirport)
             .Include(t => t.User)
             .ToListAsync();
        }
    }
}