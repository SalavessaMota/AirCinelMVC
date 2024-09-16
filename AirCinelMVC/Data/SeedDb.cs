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

            if (!_context.Manufacturers.Any())
            {
                var modelsAirbus = new List<Model>();
                modelsAirbus.Add(new Model { Name = "A319" });
                modelsAirbus.Add(new Model { Name = "A320" });
                modelsAirbus.Add(new Model { Name = "A350" });
                modelsAirbus.Add(new Model { Name = "A380" });

                _context.Manufacturers.Add(new Manufacturer
                {
                    Name = "Airbus",
                    Models = modelsAirbus
                });

                var modelsBoeing = new List<Model>();
                modelsBoeing.Add(new Model { Name = "737" });
                modelsBoeing.Add(new Model { Name = "747" });
                modelsBoeing.Add(new Model { Name = "767" });
                modelsBoeing.Add(new Model { Name = "777" });

                _context.Manufacturers.Add(new Manufacturer
                {
                    Name = "Boeing",
                    Models = modelsBoeing
                });

                await _context.SaveChangesAsync();
            }
            
            if (!_context.Airplanes.Any())
            {
                AddAirplane("A319","Airbus");
                AddAirplane("A320", "Airbus");
                AddAirplane("A350", "Airbus");
                AddAirplane("A380", "Airbus");
                AddAirplane("737", "Boeing");
                AddAirplane("747", "Boeing");
                AddAirplane("767", "Boeing");
                AddAirplane("777", "Boeing");

                await _context.SaveChangesAsync();
            }
            
            if (!_context.Countries.Any())
            {
                var citiesPortugal = new List<City>();
                citiesPortugal.Add(new City { Name = "Lisbon" });
                citiesPortugal.Add(new City { Name = "Porto" });
                citiesPortugal.Add(new City { Name = "Faro" });                

                _context.Countries.Add(new Country
                {
                    Cities = citiesPortugal,
                    Name = "Portugal",
                    Code = "PT"
                });

                var citiesSpain = new List<City>();
                citiesSpain.Add(new City { Name = "Madrid" });
                citiesSpain.Add(new City { Name = "Barcelona" });
                citiesSpain.Add(new City { Name = "Valencia" });

                _context.Countries.Add(new Country
                {
                    Cities = citiesSpain,
                    Name = "Spain",
                    Code = "ES"
                });

                await _context.SaveChangesAsync();
            }

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Customer");

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

                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }
        }

        private void AddAirplane(string model, string manufacturer)
        {
            _context.Airplanes.Add(new Airplane
            {
                Model = model,
                Capacity = _random.Next(100, 300),
                Manufacturer = manufacturer,
                YearOfManufacture = DateTime.Now.Year,
            });
        }
    }
}

