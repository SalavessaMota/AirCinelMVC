using AirCinelMVC.Data.Entities;

namespace AirCinelMVC.Data
{
    public class AirplaneRepository : GenericRepository<Airplane>, IAirplaneRepository
    {
        public AirplaneRepository(DataContext context) : base(context)
        {
        }
    }
}
