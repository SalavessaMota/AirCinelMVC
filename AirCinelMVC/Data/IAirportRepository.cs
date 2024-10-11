using AirCinelMVC.Data.Entities;
using System.Linq;

namespace AirCinelMVC.Data
{
    public interface IAirportRepository : IGenericRepository<Airport>
    {
        IQueryable<Airport> GetAllAirports();
    }
}