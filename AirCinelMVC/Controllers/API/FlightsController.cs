using AirCinelMVC.Data;
using AirCinelMVC.Data.Entities.Dtos;
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

        [HttpGet]
        public async Task<IActionResult> GetFlights()
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
    }
}