using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirCinelMVC.Data;
using AirCinelMVC.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using AirCinelMVC.Helpers;

namespace AirCinelMVC.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IAirplaneRepository _airplaneRepository;
        private readonly IAirportRepository _airportRepository;

        public FlightsController(
            IFlightRepository flightRepository,
            IAirplaneRepository airplaneRepository,
            IAirportRepository airportRepository)
        {
            _flightRepository = flightRepository;
            _airplaneRepository = airplaneRepository;
            _airportRepository = airportRepository;
        }

        // GET: Flights
        public async Task<IActionResult> Index()
        {
            return View(_flightRepository.GetAllFlightsWithAirplaneAndAirports().OrderBy(f => f.DepartureTime));
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            var flight = await _flightRepository.GetFlightWithAirplaneAndAirports(id.Value);
            if (flight == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            return View(flight);
        }

        // GET: Flights/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var airplanes = _airplaneRepository.GetAllAirplanes()
                .OrderBy(a => a.Manufacturer)
                .ThenBy(a => a.Model)
                .Select(a => new
                {
                    a.Id,
                    AirplaneName = a.Manufacturer + " - " + a.Model + " - ID: " + a.Id
                }).ToList();

            var airports = _airportRepository.GetAllAirports()
                .Include(a => a.City)
                .ThenInclude(c => c.Country)
                .OrderBy(a => a.City.Country.Name)
                .ThenBy(a => a.City.Name)
                .ThenBy(a => a.Name)
                .Select(a => new
                {
                    a.Id,
                    AirportName = a.City.Country.Name + " - " + a.City.Name + " - " + a.Name
                }).ToList();

            ViewData["AirplaneID"] = new SelectList(airplanes, "Id", "AirplaneName");
            ViewData["DepartureAirport"] = new SelectList(airports, "Id", "AirportName");
            ViewData["ArrivalAirport"] = new SelectList(airports, "Id", "AirportName");

            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AirplaneID,DepartureAirportID,ArrivalAirportID,DepartureTime,ArrivalTime")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                await _flightRepository.CreateAsync(flight);
                return RedirectToAction(nameof(Index));
            }

            var airplanes = _airplaneRepository.GetAllAirplanes()
                .OrderBy(a => a.Manufacturer)
                .ThenBy(a => a.Model)
                .Select(a => new
                {
                    a.Id,
                    AirplaneName = a.Manufacturer + " - " + a.Model + " - ID: " + a.Id
                }).ToList();

            var airports = _airportRepository.GetAllAirports()
                .Include(a => a.City)
                .ThenInclude(c => c.Country)
                .OrderBy(a => a.City.Country.Name)
                .ThenBy(a => a.City.Name)
                .ThenBy(a => a.Name)
                .Select(a => new
                {
                    a.Id,
                    AirportName = a.City.Country.Name + " - " + a.City.Name + " - " + a.Name
                }).ToList();

            ViewData["AirplaneID"] = new SelectList(airplanes, "Id", "AirplaneName", flight.AirplaneID);
            ViewData["DepartureAirportID"] = new SelectList(airports, "Id", "AirportName", flight.DepartureAirportID);
            ViewData["ArrivalAirportID"] = new SelectList(airports, "Id", "AirportName", flight.ArrivalAirportID);

            return View(flight);
        }



        // GET: Flights/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            var airplanes = _airplaneRepository.GetAllAirplanes()
                .OrderBy(a => a.Manufacturer)
                .ThenBy(a => a.Model)
                .Select(a => new
                {
                    a.Id,
                    AirplaneName = a.Manufacturer + " - " + a.Model + " - ID: " + a.Id
                }).ToList();

            var airports = _airportRepository.GetAllAirports()
                .Include(a => a.City)
                .ThenInclude(c => c.Country)
                .OrderBy(a => a.City.Country.Name)
                .ThenBy(a => a.City.Name)
                .ThenBy(a => a.Name)
                .Select(a => new
                {
                    a.Id,
                    AirportName = a.City.Country.Name + " - " + a.City.Name + " - " + a.Name
                }).ToList();

            ViewData["AirplaneID"] = new SelectList(airplanes, "Id", "AirplaneName", flight.AirplaneID);
            ViewData["DepartureAirport"] = new SelectList(airports, "Id", "AirportName", flight.DepartureAirportID);
            ViewData["ArrivalAirport"] = new SelectList(airports, "Id", "AirportName", flight.ArrivalAirportID);

            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AirplaneID,DepartureAirportID,ArrivalAirportID,DepartureTime,ArrivalTime")] Flight flight)
        {
            if (id != flight.Id)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _flightRepository.UpdateAsync(flight);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _flightRepository.ExistAsync(flight.Id))
                    {
                        return new NotFoundViewResult("FlightNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var airplanes = _airplaneRepository.GetAllAirplanes()
                .OrderBy(a => a.Manufacturer)
                .ThenBy(a => a.Model)
                .Select(a => new
                {
                    a.Id,
                    AirplaneName = a.Manufacturer + " - " + a.Model + " - ID: " + a.Id
                }).ToList();

            var airports = _airportRepository.GetAllAirports()
                .Include(a => a.City)
                .ThenInclude(c => c.Country)
                .OrderBy(a => a.City.Country.Name)
                .ThenBy(a => a.City.Name)
                .ThenBy(a => a.Name)
                .Select(a => new
                {
                    a.Id,
                    AirportName = a.City.Country.Name + " - " + a.City.Name + " - " + a.Name
                }).ToList();

            ViewData["AirplaneID"] = new SelectList(airplanes, "Id", "AirplaneName", flight.AirplaneID);
            ViewData["DepartureAirport"] = new SelectList(airports, "Id", "AirportName", flight.DepartureAirportID);
            ViewData["ArrivalAirport"] = new SelectList(airports, "Id", "AirportName", flight.ArrivalAirportID);

            return View(flight);
        }

        // GET: Flights/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            var flight = await _flightRepository.GetByIdAsync(id.Value);
            if (flight == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            try
            {
                await _flightRepository.DeleteAsync(flight);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ViewBag.ErrorTitle = $"This flight is probably being used!!";
                ViewBag.ErrorMessage = $"It can not be deleted because there are tickets that use it.</br></br>" +
                                       $"If you want to delete this flight, please remove the tickets that are using it," +
                                       $"and then try to delete it again.";
            }

            return View("Error");
        }

        //// POST: Flights/Delete/5
        //[Authorize(Roles = "Admin")]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var flight = await _context.Flights.FindAsync(id);
        //    _context.Flights.Remove(flight);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


        public IActionResult FlightNotFound()
        {
            return View();
        }
    }
}
