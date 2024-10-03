using AirCinelMVC.Data;
using AirCinelMVC.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace AirCinelMVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FlightsController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IUserHelper _userHelper;

        public FlightsController(
            IFlightRepository flightRepository,
            IUserHelper userHelper)
        {
            _flightRepository = flightRepository;
            _userHelper = userHelper;
        }

        [HttpGet]
        public IActionResult GetFlights()
        {
            var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userHelper.GetUserByEmailAsync(userEmail).Result;

            if (user == null)
            {
                return Unauthorized();
            }

            var futureFlights = _flightRepository.GetFlightsByUserId(user.Id)
                .Where(f => f.DepartureTime >= DateTime.Now)
                .OrderBy(f => f.DepartureTime)
                .ToList();

            return Ok(futureFlights);
        }
    }
}
