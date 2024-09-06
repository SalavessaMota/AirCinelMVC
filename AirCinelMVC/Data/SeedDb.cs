using AirCinelMVC.Data.Entities;
using AirCinelMVC.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AirCinelMVC.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country { Name = "Portugal", Code = "PT" });
                await _context.SaveChangesAsync();
            }

            
            var country = _context.Countries.FirstOrDefault(c => c.Name == "Portugal");

            
            if (!_context.Cities.Any())
            {
                _context.Cities.Add(new City { Name = "Lisboa", CountryID = country.Id });
                await _context.SaveChangesAsync();
            }

            
            var city = _context.Cities.FirstOrDefault(c => c.Name == "Lisboa");

            
            var user = await _userHelper.GetUserByEmailAsync("nunosalavessa@hotmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Nuno",
                    LastName = "Salavessa",
                    Email = "nunosalavessa@hotmail.com",
                    Address = "Rua Jau 33",
                    UserName = "nunosalavessa@hotmail.com",
                    PhoneNumber = "123456789",
                    CityId = city.Id,  
                    City = city
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

            
            if (!_context.Airplanes.Any())
            {
                AddAirplane("A319", user);
                AddAirplane("A320", user);
                AddAirplane("A350", user);
                AddAirplane("A380", user);
                await _context.SaveChangesAsync();
            }
        }

        private void AddAirplane(string model, User user)
        {
            _context.Airplanes.Add(new Airplane
            {
                Model = model,
                Capacity = _random.Next(100, 300),
                Manufacturer = "Airbus",
                YearOfManufacture = DateTime.Now.Year,
                User = user
            });
        }
    }
}

