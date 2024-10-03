using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AirCinelMVC.Models;
using AirCinelMVC.Data;
using System.Linq;
using System;

namespace AirCinelMVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IFlightRepository _flightRepository;

        public HomeController(
			ILogger<HomeController> logger,
			IFlightRepository flightRepository)
		{
			_logger = logger;
            _flightRepository = flightRepository;
        }

		public IActionResult Index()
		{
            var futureFlights = _flightRepository.GetAllFlightsWithAirplaneAndAirports()
                .Where(f => f.DepartureTime >= DateTime.Now)
                .OrderBy(f => f.DepartureTime);
            return View(futureFlights);
        }

		public IActionResult Destinations()
        {
            return View();
        }

		public IActionResult AboutUs()
        {
            return View();
        }

		public IActionResult Privacy()
		{
			return View();
		}
	}
}
