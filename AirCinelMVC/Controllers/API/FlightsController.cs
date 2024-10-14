using AirCinelMVC.Data;
using AirCinelMVC.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

            return Ok(futureFlights);
        }
    }
}