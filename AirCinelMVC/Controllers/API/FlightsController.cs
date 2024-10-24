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
        private readonly IUserHelper _userHelper;

        public FlightsController(
            IFlightRepository flightRepository,
            IUserRepository userRepository,
            IUserHelper userHelper)
        {
            _flightRepository = flightRepository;
            _userRepository = userRepository;
            _userHelper = userHelper;
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
    }
}