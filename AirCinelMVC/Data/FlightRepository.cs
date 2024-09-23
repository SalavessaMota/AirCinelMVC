using AirCinelMVC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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

        public bool ExistFlight(int id)
        {
            return _context.Flights.Any(f => f.Id == id);
        }
    }
}
