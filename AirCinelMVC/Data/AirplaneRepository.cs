using AirCinelMVC.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AirCinelMVC.Data
{
    public class AirplaneRepository : GenericRepository<Airplane>, IAirplaneRepository
    {
        private readonly DataContext _context;

        public AirplaneRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Airplane> GetAllWithUsers()
        {
            return _context.Airplanes.Include(a => a.User);
        }

    }
}
