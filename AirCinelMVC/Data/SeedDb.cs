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
        private readonly IUserRepository _userRepository;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserRepository userRepository, IUserHelper userHelper)
        {
            _context = context;
            _userRepository = userRepository;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();

            if (!_context.Manufacturers.Any())
            {
                var modelsAirbus = new List<Model>();
                modelsAirbus.Add(new Model { Name = "A319" });
                modelsAirbus.Add(new Model { Name = "A320" });
                modelsAirbus.Add(new Model { Name = "A330" });
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
                modelsBoeing.Add(new Model { Name = "757" });
                modelsBoeing.Add(new Model { Name = "767" });
                modelsBoeing.Add(new Model { Name = "777" });

                _context.Manufacturers.Add(new Manufacturer
                {
                    Name = "Boeing",
                    Models = modelsBoeing
                });

                var modelsEmbraer = new List<Model>();
                modelsEmbraer.Add(new Model { Name = "E170" });
                modelsEmbraer.Add(new Model { Name = "E175" });
                modelsEmbraer.Add(new Model { Name = "E190" });
                modelsEmbraer.Add(new Model { Name = "E195" });

                _context.Manufacturers.Add(new Manufacturer
                {
                    Name = "Embraer",
                    Models = modelsEmbraer
                });

                await _context.SaveChangesAsync();
            }

            if (!_context.Airplanes.Any())
            {
                AddAirplane("A319", "Airbus", 156);
                AddAirplane("A320", "Airbus", 186);
                AddAirplane("A330", "Airbus", 440);
                AddAirplane("A350", "Airbus", 440);
                AddAirplane("A380", "Airbus", 350);
                AddAirplane("737", "Boeing", 230);
                AddAirplane("747", "Boeing", 660);
                AddAirplane("757", "Boeing", 295);
                AddAirplane("767", "Boeing", 375);
                AddAirplane("777", "Boeing", 550);
                AddAirplane("E170", "Embraer", 78);
                AddAirplane("E175", "Embraer", 88);
                AddAirplane("E190", "Embraer", 114);
                AddAirplane("E195", "Embraer", 124);

                await _context.SaveChangesAsync();
            }

            if (!_context.Countries.Any())
            {
                // Portugal
                var citiesPortugal = new List<City>
                {
                    new City { Name = "Lisbon", Airports = new List<Airport> { new Airport { Name = "Humberto Delgado Airport (LIS)" } } },
                    new City { Name = "Porto", Airports = new List<Airport> { new Airport { Name = "Francisco Sá Carneiro Airport (OPO)" } } },
                    new City { Name = "Faro", Airports = new List<Airport> { new Airport { Name = "Faro Airport (FAO)" } } }
                };

                _context.Countries.Add(new Country
                {
                    Cities = citiesPortugal,
                    Name = "Portugal",
                    Code = "PT"
                });

                // Spain
                var citiesSpain = new List<City>
                {
                    new City { Name = "Madrid", Airports = new List<Airport> { new Airport { Name = "Adolfo Suárez Madrid–Barajas Airport (MAD)" } } },
                    new City { Name = "Barcelona", Airports = new List<Airport> { new Airport { Name = "Barcelona–El Prat Airport (BCN)" } } },
                    new City { Name = "Valencia", Airports = new List<Airport> { new Airport { Name = "Valencia Airport (VLC)" } } }
                };

                _context.Countries.Add(new Country
                {
                    Cities = citiesSpain,
                    Name = "Spain",
                    Code = "ES"
                });

                // France
                var citiesFrance = new List<City>
                {
                    new City { Name = "Paris", Airports = new List<Airport> { new Airport { Name = "Charles de Gaulle Airport (CDG)" }, new Airport { Name = "Orly Airport (ORY)" } } },
                    new City { Name = "Nice", Airports = new List<Airport> { new Airport { Name = "Nice Côte d'Azur Airport (NCE)" } } },
                    new City { Name = "Lyon", Airports = new List<Airport> { new Airport { Name = "Lyon–Saint-Exupéry Airport (LYS)" } } }
                };

                _context.Countries.Add(new Country
                {
                    Cities = citiesFrance,
                    Name = "France",
                    Code = "FR"
                });

                // Germany
                var citiesGermany = new List<City>
                {
                    new City { Name = "Berlin", Airports = new List<Airport> { new Airport { Name = "Berlin Brandenburg Airport (BER)" } } },
                    new City { Name = "Frankfurt", Airports = new List<Airport> { new Airport { Name = "Frankfurt Airport (FRA)" } } },
                    new City { Name = "Munich", Airports = new List<Airport> { new Airport { Name = "Munich Airport (MUC)" } } }
                };

                _context.Countries.Add(new Country
                {
                    Cities = citiesGermany,
                    Name = "Germany",
                    Code = "DE"
                });

                // Italy
                var citiesItaly = new List<City>
                {
                    new City { Name = "Rome", Airports = new List<Airport> { new Airport { Name = "Leonardo da Vinci–Fiumicino Airport (FCO)" } } },
                    new City { Name = "Milan", Airports = new List<Airport> { new Airport { Name = "Milan Malpensa Airport (MXP)" } } },
                    new City { Name = "Venice", Airports = new List<Airport> { new Airport { Name = "Venice Marco Polo Airport (VCE)" } } }
                };

                _context.Countries.Add(new Country
                {
                    Cities = citiesItaly,
                    Name = "Italy",
                    Code = "IT"
                });

                // Netherlands
                var citiesNetherlands = new List<City>
                {
                    new City { Name = "Amsterdam", Airports = new List<Airport> { new Airport { Name = "Amsterdam Schiphol Airport (AMS)" } } },
                    new City { Name = "Rotterdam", Airports = new List<Airport> { new Airport { Name = "Rotterdam The Hague Airport (RTM)" } } },
                    new City { Name = "Eindhoven", Airports = new List<Airport> { new Airport { Name = "Eindhoven Airport (EIN)" } } }
                };

                _context.Countries.Add(new Country
                {
                    Cities = citiesNetherlands,
                    Name = "Netherlands",
                    Code = "NL"
                });

                // Switzerland
                var citiesSwitzerland = new List<City>
                {
                    new City { Name = "Zurich", Airports = new List<Airport> { new Airport { Name = "Zurich Airport (ZRH)" } } },
                    new City { Name = "Geneva", Airports = new List<Airport> { new Airport { Name = "Geneva Airport (GVA)" } } },
                    new City { Name = "Basel", Airports = new List<Airport> { new Airport { Name = "EuroAirport Basel-Mulhouse-Freiburg (BSL)" } } }
                };

                _context.Countries.Add(new Country
                {
                    Cities = citiesSwitzerland,
                    Name = "Switzerland",
                    Code = "CH"
                });

                // Greece
                var citiesGreece = new List<City>
                {
                    new City { Name = "Athens", Airports = new List<Airport> { new Airport { Name = "Athens International Airport (ATH)" } } },
                    new City { Name = "Thessaloniki", Airports = new List<Airport> { new Airport { Name = "Thessaloniki Airport (SKG)" } } },
                    new City { Name = "Heraklion", Airports = new List<Airport> { new Airport { Name = "Heraklion International Airport (HER)" } } }
                };

                _context.Countries.Add(new Country
                {
                    Cities = citiesGreece,
                    Name = "Greece",
                    Code = "GR"
                });

                await _context.SaveChangesAsync();
            }

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Customer");

            var user = await _userRepository.GetUserByEmailAsync("nunosalavessa@hotmail.com");
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

                //var result = await _userHelper.AddUserAsync(user, "123123");
                var result = await _userRepository.AddUserAsync(user, "Aircinel1!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }
        }

        private void AddAirplane(string model, string manufacturer, int capacity)
        {
            _context.Airplanes.Add(new Airplane
            {
                Model = model,
                Capacity = capacity,
                Manufacturer = manufacturer,
                YearOfManufacture = DateTime.Now.Year,
                ModelId = _context.Models.FirstOrDefault(m => m.Name == model).Id
            });
        }
    }
}