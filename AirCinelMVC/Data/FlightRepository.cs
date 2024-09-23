using AirCinelMVC.Data.Entities;
using Microsoft.EntityFrameworkCore;
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

        public bool ExistFlight(int id)
        {
            return _context.Flights.Any(f => f.Id == id);
        }
    }
}
