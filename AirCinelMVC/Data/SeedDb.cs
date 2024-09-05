using System;
using System.Linq;
using System.Threading.Tasks;

namespace AirCinelMVC.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private Random _random;

        public SeedDb(DataContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if (!_context.Airplanes.Any())
            {
                AddAirplane("A319");
                AddAirplane("A320");
                AddAirplane("A350");
                AddAirplane("A380");
                await _context.SaveChangesAsync();
            }
        }

        private void AddAirplane(string model)
        {
            _context.Airplanes.Add(new Entities.Airplane
            {
                Model = model,
                Capacity = _random.Next(100, 300),
                Manufacturer = "Airbus",
                YearOfManufacture = DateTime.Now.Year
            });
        }
    }
}
