using AirCinelMVC.Data;
using AirCinelMVC.Data.Dtos;
using AirCinelMVC.Data.Entities;
using AirCinelMVC.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AirCinelMVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FlightsController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAirportRepository _airportRepository;
        private readonly IUserHelper _userHelper;
        private readonly ISeatHelper _seatHelper;

        public FlightsController(
            IFlightRepository flightRepository,
            IUserRepository userRepository,
            IAirportRepository airportRepository,
            IUserHelper userHelper,
            ISeatHelper seatHelper)
        {
            _flightRepository = flightRepository;
            _userRepository = userRepository;
            _airportRepository = airportRepository;
            _userHelper = userHelper;
            _seatHelper = seatHelper;
        }

        [HttpGet("future")]
        [Authorize]
        public async Task<IActionResult> GetFutureFlightsForUser()
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByEmailAsync(userEmail);

            if (user == null)
            {
                return Unauthorized();
            }

            var futureFlights = _flightRepository.GetFlightsByUserId(user.Id)
                .Where(f => f.DepartureTime >= DateTime.Now)
                .OrderBy(f => f.DepartureTime)
                .ToList();

            var flightDtos = futureFlights.Select(flight => new FlightDto
            {
                Id = flight.Id,
                FlightNumber = flight.FlightNumber,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                Airplane = new AirplaneDto
                {
                    Id = flight.Airplane.Id,
                    Model = flight.Airplane.Model,
                    Manufacturer = flight.Airplane.Manufacturer,
                    Capacity = flight.Airplane.Capacity,
                    ImageFullPath = flight.Airplane.ImageFullPath
                },
                DepartureAirport = new AirportDto
                {
                    Id = flight.DepartureAirport.Id,
                    Name = flight.DepartureAirport.Name,
                    ImageFullPath = flight.DepartureAirport.ImageFullPath
                },
                ArrivalAirport = new AirportDto
                {
                    Id = flight.ArrivalAirport.Id,
                    Name = flight.ArrivalAirport.Name,
                    ImageFullPath = flight.ArrivalAirport.ImageFullPath
                },
                Tickets = flight.Tickets?.Select(ticket => new TicketDto
                {
                    Id = ticket.Id,
                    SeatNumber = ticket.SeatNumber,
                    User = ticket.User == null ? null : new UserDto
                    {
                        FirstName = ticket.User.FirstName,
                        LastName = ticket.User.LastName,
                        Email = ticket.User.Email,
                        ImageFullPath = ticket.User.ImageFullPath
                    }
                }).ToList() ?? new List<TicketDto>()
            }).ToList();

            return Ok(flightDtos);
        }

        [HttpGet("history")]
        [Authorize]
        public async Task<IActionResult> GetFlightHistoryForUser()
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByEmailAsync(userEmail);

            if (user == null)
            {
                return Unauthorized();
            }

            var pastFlights = _flightRepository.GetFlightsByUserId(user.Id)
                .Where(f => f.DepartureTime <= DateTime.Now)
                .OrderBy(f => f.DepartureTime)
                .ToList();

            var flightDtos = pastFlights.Select(flight => new FlightDto
            {
                Id = flight.Id,
                FlightNumber = flight.FlightNumber,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                Airplane = new AirplaneDto
                {
                    Id = flight.Airplane.Id,
                    Model = flight.Airplane.Model,
                    Manufacturer = flight.Airplane.Manufacturer,
                    Capacity = flight.Airplane.Capacity,
                    ImageFullPath = flight.Airplane.ImageFullPath
                },
                DepartureAirport = new AirportDto
                {
                    Id = flight.DepartureAirport.Id,
                    Name = flight.DepartureAirport.Name,
                    ImageFullPath = flight.DepartureAirport.ImageFullPath
                },
                ArrivalAirport = new AirportDto
                {
                    Id = flight.ArrivalAirport.Id,
                    Name = flight.ArrivalAirport.Name,
                    ImageFullPath = flight.ArrivalAirport.ImageFullPath
                },
                Tickets = flight.Tickets?.Select(ticket => new TicketDto
                {
                    Id = ticket.Id,
                    SeatNumber = ticket.SeatNumber,
                    User = ticket.User == null ? null : new UserDto
                    {
                        FirstName = ticket.User.FirstName,
                        LastName = ticket.User.LastName,
                        Email = ticket.User.Email,
                        ImageFullPath = ticket.User.ImageFullPath
                    }
                }).ToList() ?? new List<TicketDto>()
            }).ToList();

            return Ok(flightDtos);
        }

        [HttpGet("available")]
        [AllowAnonymous]
        public IActionResult GetAvailableFlightsForAnonymous()
        {
            var availableFlights = _flightRepository.GetAllFlightsWithAirplaneAirportsAndTickets()
                .Where(f => f.DepartureTime >= DateTime.Now)
                .OrderBy(f => f.DepartureTime)
                .ToList();

            var flightDtos = availableFlights.Select(flight => new FlightDto
            {
                Id = flight.Id,
                FlightNumber = flight.FlightNumber,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                Airplane = new AirplaneDto
                {
                    Id = flight.Airplane.Id,
                    Model = flight.Airplane.Model,
                    Manufacturer = flight.Airplane.Manufacturer,
                    Capacity = flight.Airplane.Capacity,
                    ImageFullPath = flight.Airplane.ImageFullPath
                },
                DepartureAirport = new AirportDto
                {
                    Id = flight.DepartureAirport.Id,
                    Name = flight.DepartureAirport.Name,
                    ImageFullPath = flight.DepartureAirport.ImageFullPath
                },
                ArrivalAirport = new AirportDto
                {
                    Id = flight.ArrivalAirport.Id,
                    Name = flight.ArrivalAirport.Name,
                    ImageFullPath = flight.ArrivalAirport.ImageFullPath
                }
            }).ToList() ?? new List<FlightDto>();

            return Ok(flightDtos);
        }

        [HttpPost("purchase")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PurchaseTicketAPI([FromBody] PurchaseTicketDto model)
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByEmailAsync(userEmail);

            if (user == null)
            {
                return Unauthorized(new { Message = "User not found." });
            }

            // Verificar se o voo existe e incluir os dados necessários
            var flight = await _flightRepository.GetFlightWithAirplaneAirportsAndTicketsAsync(model.FlightId);

            if (flight == null)
            {
                return NotFound(new { Message = "Flight not found" });
            }

            if (flight.DepartureTime < DateTime.Now)
            {
                return BadRequest(new { Message = "Not possible to buy tickets for past flights." });
            }

            if (flight.Tickets.Any(t => t.SeatNumber == model.SeatNumber))
            {
                return BadRequest(new { Message = "The seat is already taken." });
            }

            if (flight.Tickets.Count >= flight.Airplane.Capacity)
            {
                return BadRequest(new { Message = "No more available tickets for this flight." });
            }

            var ticket = new Ticket
            {
                FlightId = model.FlightId,
                SeatNumber = model.SeatNumber,
                UserId = user.Id
            };

            flight.Tickets.Add(ticket);
            await _flightRepository.UpdateAsync(flight);

            return Ok(new { Message = "Ticket bought successfully", TicketId = ticket.Id });
        }

        [HttpGet("{flightId}/availableseats")]
        [Authorize]
        public async Task<IActionResult> GetAvailableSeatsForFlight(int flightId)
        {
            // Verificar se o voo existe
            var flight = await _flightRepository.GetFlightWithAirplaneAirportsAndTicketsAsync(flightId);

            if (flight == null)
            {
                return NotFound(new { Message = "Flight not found." });
            }

            // Obter o número de lugares por fila com base no modelo do avião
            if(flight.Airplane == null)
            {
                return BadRequest(new { Message = "Airplane not found." });
            }

            int seatsPerRow = _seatHelper.GetSeatsPerRowByModel(flight.Airplane.Model);
            
            
            int totalSeats = flight.Airplane.Capacity;
            int totalRows = (int)Math.Ceiling((double)totalSeats / seatsPerRow);

            // Gerar a lista de todos os lugares possíveis
            var seatLetters = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" }; // Até 12 lugares por fila
            var allSeats = new List<string>();

            for (int row = 1; row <= totalRows; row++)
            {
                for (int col = 0; col < seatsPerRow; col++)
                {
                    if (col >= seatLetters.Length) break; // Prevenir exceder o limite de letras
                    allSeats.Add($"{row}{seatLetters[col]}");
                }
            }

            // Obter a lista de lugares ocupados
            var occupiedSeats = flight.Tickets.Select(t => t.SeatNumber).ToList();

            // Filtrar os lugares disponíveis
            var availableSeats = allSeats.Except(occupiedSeats).ToList();

            return Ok(availableSeats);
        }



        [HttpGet("tickets")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetUserTickets()
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userRepository.GetUserByEmailAsync(userEmail);

            if (user == null)
            {
                return Unauthorized(new { Message = "User not found." });
            }

            var tickets = _flightRepository.GetTicketsByUserIdAsync(user.Id).Result;

            if (tickets == null || !tickets.Any())
            {
                return NotFound(new { Message = "No tickets found for this user." });
            }

            var ticketDtos = tickets.Select(ticket => new BoughtTicketDto
            {
                Id = ticket.Id,
                SeatNumber = ticket.SeatNumber,
                Flight = new FlightDto
                {
                    Id = ticket.Flight.Id,
                    FlightNumber = ticket.Flight.FlightNumber,
                    DepartureTime = ticket.Flight.DepartureTime,
                    ArrivalTime = ticket.Flight.ArrivalTime,
                    Airplane = new AirplaneDto
                    {
                        Id = ticket.Flight.Airplane.Id,
                        Model = ticket.Flight.Airplane.Model,
                        Manufacturer = ticket.Flight.Airplane.Manufacturer,
                        Capacity = ticket.Flight.Airplane.Capacity,
                        ImageFullPath = ticket.Flight.Airplane.ImageFullPath
                    },
                    DepartureAirport = new AirportDto
                    {
                        Id = ticket.Flight.DepartureAirport.Id,
                        Name = ticket.Flight.DepartureAirport.Name,
                        ImageFullPath = ticket.Flight.DepartureAirport.ImageFullPath
                    },
                    ArrivalAirport = new AirportDto
                    {
                        Id = ticket.Flight.ArrivalAirport.Id,
                        Name = ticket.Flight.ArrivalAirport.Name,
                        ImageFullPath = ticket.Flight.ArrivalAirport.ImageFullPath
                    }
                }
            }).ToList();

            return Ok(ticketDtos);
        }


        [HttpGet("filter")]
        [AllowAnonymous]
        public IActionResult GetFilteredFlights(
    [FromQuery] string? departureCity = null,
    [FromQuery] string? arrivalCity = null)
        {
            // Obter todos os voos futuros
            var flights = _flightRepository.GetAllFlightsWithAirplaneAirportsAndTickets()
                                           .Where(f => f.DepartureTime >= DateTime.Now);

            // Filtrar por cidade de partida, se fornecido
            if (!string.IsNullOrEmpty(departureCity))
            {
                departureCity = departureCity.ToLower(); // Convertendo para minúsculas
                flights = flights.Where(f => f.DepartureAirport.City.Name.ToLower() == departureCity);
            }

            // Filtrar por cidade de chegada, se fornecido
            if (!string.IsNullOrEmpty(arrivalCity))
            {
                arrivalCity = arrivalCity.ToLower(); // Convertendo para minúsculas
                flights = flights.Where(f => f.ArrivalAirport.City.Name.ToLower() == arrivalCity);
            }

            // Mapear os voos filtrados para DTOs
            var flightDtos = flights.OrderBy(f => f.DepartureTime)
                                    .Select(flight => new FlightDto
                                    {
                                        Id = flight.Id,
                                        FlightNumber = flight.FlightNumber,
                                        DepartureTime = flight.DepartureTime,
                                        ArrivalTime = flight.ArrivalTime,
                                        Airplane = new AirplaneDto
                                        {
                                            Id = flight.Airplane.Id,
                                            Model = flight.Airplane.Model,
                                            Manufacturer = flight.Airplane.Manufacturer,
                                            Capacity = flight.Airplane.Capacity,
                                            ImageFullPath = flight.Airplane.ImageFullPath
                                        },
                                        DepartureAirport = new AirportDto
                                        {
                                            Id = flight.DepartureAirport.Id,
                                            Name = flight.DepartureAirport.Name,
                                            ImageFullPath = flight.DepartureAirport.ImageFullPath
                                        },
                                        ArrivalAirport = new AirportDto
                                        {
                                            Id = flight.ArrivalAirport.Id,
                                            Name = flight.ArrivalAirport.Name,
                                            ImageFullPath = flight.ArrivalAirport.ImageFullPath
                                        }
                                    })
                                    .ToList();

            return Ok(flightDtos);
        }


        [HttpGet("cities")]
        [AllowAnonymous]
        public IActionResult GetCities()
        {
            // Obter todas as cidades únicas dos aeroportos
            var cities = _airportRepository.GetAllAirports()
                                           .Select(a => a.City)
                                           .Distinct()
                                           .Select(city => new City
                                           {
                                               Id = city.Id,
                                               Name = city.Name
                                           })
                                           .ToList();

            return Ok(cities);
        }
    }
}