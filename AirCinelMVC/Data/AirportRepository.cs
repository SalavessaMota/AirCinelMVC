using AirCinelMVC.Data.Entities;
using System.Linq;

namespace AirCinelMVC.Data
{
    public class AirportRepository : GenericRepository<Airport>, IAirportRepository
    {
        private readonly DataContext _context;

        public AirportRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Airport> GetAllAirports()
        {
            return _context.Airports;
        }
    }
}