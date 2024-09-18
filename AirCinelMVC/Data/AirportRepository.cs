using AirCinelMVC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<Airport> GetAirportByNameAsync(string name)
        {
            return await _context.Airports
                .Where(a => a.Name == name)
                .FirstOrDefaultAsync();
        }
    }
}
