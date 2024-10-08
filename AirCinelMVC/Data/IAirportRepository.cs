using AirCinelMVC.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace AirCinelMVC.Data
{
    public interface IAirportRepository : IGenericRepository<Airport>
    {
        public IQueryable<Airport> GetAllAirports();
    }
}