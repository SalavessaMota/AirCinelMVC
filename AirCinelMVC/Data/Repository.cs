using AirCinelMVC.Data.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirCinelMVC.Data
{
    public class Repository : IRepository
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Airplane> GetAirplanes()
        {
            return _context.Airplanes.OrderBy(a => a.Model);
        }

        public Airplane GetAirplane(int id)
        {
            return _context.Airplanes.Find(id);
        }

        public void AddAirplane(Airplane airplane)
        {
            _context.Airplanes.Add(airplane);
        }

        public void UpdateAirplane(Airplane airplane)
        {
            _context.Airplanes.Update(airplane);
        }

        public void RemoveAirplane(Airplane airplane)
        {
            _context.Airplanes.Remove(airplane);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool AirplaneExists(int id)
        {
            return _context.Airplanes.Any(a => a.Id == id);
        }
    }
}
