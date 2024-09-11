using AirCinelMVC.Data.Entities;
using AirCinelMVC.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
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
                var cities = new List<City>();
                cities.Add(new City { Name = "Lisbon" });
                cities.Add(new City { Name = "Porto" });
                cities.Add(new City { Name = "Faro" });

                _context.Countries.Add(new Country
                {
                    Cities = cities,
                    Name = "Portugal"
                });

                await _context.SaveChangesAsync();
            }

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
                    CityId = _context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault()
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

