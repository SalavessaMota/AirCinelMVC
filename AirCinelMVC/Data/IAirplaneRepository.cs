using AirCinelMVC.Data.Entities;
using System.Linq;

namespace AirCinelMVC.Data
{
    public interface IAirplaneRepository : IGenericRepository<Airplane>
    {
        public IQueryable<Airplane> GetAllWithUsers();
    }
}
