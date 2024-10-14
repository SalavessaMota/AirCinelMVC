using AirCinelMVC.Data;
using AirCinelMVC.Data.Entities;
using AirCinelMVC.Helpers;
using AirCinelMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Barcode;
using Syncfusion.Pdf.Graphics;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AirCinelMVC.Controllers
{
    public class FlightsController : Controller
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IAirplaneRepository _airplaneRepository;
        private readonly IAirportRepository _airportRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserHelper _userHelper;
        private readonly ISeatHelper _seatHelper;

        public FlightsController(
            IFlightRepository flightRepository,
            IAirplaneRepository airplaneRepository,
            IAirportRepository airportRepository,
            IUserRepository userRepository,
            IUserHelper userHelper,
            ISeatHelper seatHelper)
        {
            _flightRepository = flightRepository;
            _airplaneRepository = airplaneRepository;
            _airportRepository = airportRepository;
            _userRepository = userRepository;
            _userHelper = userHelper;
            _seatHelper = seatHelper;
        }

        // GET: Flights
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Index()
        {
            return View(_flightRepository.GetAllFlightsWithAirplaneAndAirports().OrderBy(f => f.DepartureTime));
        }

        public async Task<IActionResult> FlightHistory()
        {
            var pastFlights = _flightRepository.GetAllFlightsWithAirplaneAndAirports()
                .Where(f => f.DepartureTime < DateTime.Now)
                .OrderByDescending(f => f.DepartureTime);
            return View(pastFlights);
        }

        public IActionResult UpcomingFlights()
        {
            var futureFlights = _flightRepository.GetAllFlightsWithAirplaneAndAirports()
                .Where(f => f.DepartureTime >= DateTime.Now)
                .OrderBy(f => f.DepartureTime);
            return View(futureFlights);
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UserUpcomingFlights()
        {
            var currentUser = await _userRepository.GetUserByEmailAsync(User.Identity.Name);

            var futureFlights = _flightRepository.GetFlightsByUserId(currentUser.Id)
                .Where(f => f.DepartureTime >= DateTime.Now)
                .OrderBy(f => f.DepartureTime)
                .ToList();
            return View(futureFlights);
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UserFlightHistory()
        {
            var currentUser = await _userRepository.GetUserByEmailAsync(User.Identity.Name);

            var futureFlights = _flightRepository.GetFlightsByUserId(currentUser.Id)
                .Where(f => f.DepartureTime < DateTime.Now)
                .OrderBy(f => f.DepartureTime)
                .ToList();
            return View(futureFlights);
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("NotAuthorized", "Account");
            }
            if (id == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            var flight = await _flightRepository.GetFlightWithAirplaneAndAirportsAsync(id.Value);
            if (flight == null)
            {
                return new NotFoundViewResult("FlightNotFound");
            }

            return View(flight);
        }

        // GET: Flights/Create
        [Authorize(Roles = "Employee")]
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

        [Authorize(Roles = "Employee")]
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
        [Authorize(Roles = "Employee")]
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
        [Authorize(Roles = "Employee")]
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
        [Authorize(Roles = "Employee")]
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

        public IActionResult TicketNotFound()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PurchaseTicket(int flightId)
        {
            var flight = await _flightRepository.GetFlightWithAirplaneAirportsAndTicketsAsync(flightId);

            if (flight == null)
            {
                return new NotFoundViewResult("TicketNotFound");
            }

            var seatMap = _seatHelper.GenerateSeatMap(flight.Airplane.Model, flight.Airplane.Capacity);

            var occupiedSeats = flight.Tickets.Select(t => t.SeatNumber).ToList();

            var model = new PurchaseTicketViewModel
            {
                FlightId = flight.Id,
                SeatMap = seatMap,
                OccupiedSeats = occupiedSeats
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PurchaseTicket(PurchaseTicketViewModel model)
        {
            var flight = await _flightRepository.GetFlightWithAirplaneAirportsAndTicketsAsync(model.FlightId);

            if (flight == null)
            {
                return new NotFoundViewResult("TicketNotFound");
            }

            if (flight.Tickets.Any(t => t.SeatNumber == model.SelectedSeat))
            {
                ModelState.AddModelError("", "The selected seat is already reserved.");
                return View(model);
            }

            if (flight.Tickets.Count >= flight.Airplane.Capacity)
            {
                ModelState.AddModelError("", "No more tickets available for this flight.");
                return View(model);
            }

            var user = await _userRepository.GetUserByEmailAsync(User.Identity.Name);
            var ticket = new Ticket
            {
                FlightId = model.FlightId,
                SeatNumber = model.SelectedSeat,
                UserId = user.Id
            };

            flight.Tickets.Add(ticket);
            await _flightRepository.UpdateAsync(flight);

            return RedirectToAction("NewTicketDetails", new { id = ticket.Id });
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> NewTicketDetails(int id)
        {
            var ticket = await _flightRepository.GetTicketWithUserFlightAirplaneAndAirportsAsync(id);

            if (ticket == null)
            {
                return new NotFoundViewResult("TicketNotFound");
            }

            return View(ticket);
        }

        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> TicketDetails(int id)
        {
            var ticket = await _flightRepository.GetTicketWithUserFlightAirplaneAndAirportsAsync(id);

            if (ticket == null)
            {
                return new NotFoundViewResult("TicketNotFound");
            }

            return View(ticket);
        }

        [Authorize(Roles = "Employee,Customer")]
        public async Task<IActionResult> PrintTicket(int id)
        {
            var ticket = await _flightRepository.GetTicketWithUserFlightAirplaneAndAirportsAsync(id);

            if (ticket == null)
            {
                return new NotFoundViewResult("TicketNotFound");
            }

            PdfDocument document = new PdfDocument();
            PdfPage page = document.Pages.Add();
            PdfGraphics graphics = page.Graphics;

            PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 20);
            PdfFont subtitleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
            PdfFont normalFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
            PdfBrush brush = PdfBrushes.Black;

            string logoUrl = "https://aircinelmvc.blob.core.windows.net/resources/logoAirCinel.jpg";

            using (var client = new HttpClient())
            {
                var imageBytes = await client.GetByteArrayAsync(logoUrl);
                using (var logoStream = new MemoryStream(imageBytes))
                {
                    PdfBitmap logoImage = new PdfBitmap(logoStream);
                    graphics.DrawImage(logoImage, new RectangleF(50, 10, 300, 50));
                }
            }

            graphics.DrawString("AirCinel Boarding Pass", titleFont, brush, new PointF(10, 70));

            PdfPen pen = new PdfPen(PdfBrushes.Gray, 0.5f);
            graphics.DrawLine(pen, new PointF(0, 100), new PointF(page.GetClientSize().Width, 100));

            float currentY = 110;
            float padding = 20;

            graphics.DrawString("Departure Airport:", subtitleFont, brush, new PointF(10, currentY));
            currentY += padding;
            graphics.DrawString(ticket.Flight.DepartureAirport.Name, normalFont, brush, new PointF(10, currentY));

            currentY += padding;
            graphics.DrawString("Arrival Airport:", subtitleFont, brush, new PointF(10, currentY));
            currentY += padding;
            graphics.DrawString(ticket.Flight.ArrivalAirport.Name, normalFont, brush, new PointF(10, currentY));

            currentY += padding;
            graphics.DrawString("Gate:", subtitleFont, brush, new PointF(10, currentY));
            currentY += padding;
            graphics.DrawString("C3", normalFont, brush, new PointF(10, currentY));

            currentY += padding;
            graphics.DrawString("Terminal:", subtitleFont, brush, new PointF(10, currentY));
            currentY += padding;
            graphics.DrawString("W", normalFont, brush, new PointF(10, currentY));

            currentY += padding;
            graphics.DrawString("Seat Number:", subtitleFont, brush, new PointF(10, currentY));
            currentY += padding;
            graphics.DrawString(ticket.SeatNumber, normalFont, brush, new PointF(10, currentY));

            currentY += padding;
            graphics.DrawString("Class:", subtitleFont, brush, new PointF(10, currentY));
            currentY += padding;
            graphics.DrawString("E", normalFont, brush, new PointF(10, currentY));

            currentY += padding;
            graphics.DrawString("Departure Time:", subtitleFont, brush, new PointF(10, currentY));
            currentY += padding;
            graphics.DrawString(ticket.Flight.DepartureTime.ToString("dd/MM/yyyy HH:mm"), normalFont, brush, new PointF(10, currentY));

            currentY += padding;
            graphics.DrawString("Arrival Time:", subtitleFont, brush, new PointF(10, currentY));
            currentY += padding;
            graphics.DrawString(ticket.Flight.ArrivalTime.ToString("dd/MM/yyyy HH:mm"), normalFont, brush, new PointF(10, currentY));

            currentY += padding;
            graphics.DrawString("Passenger:", subtitleFont, brush, new PointF(10, currentY));
            currentY += padding;
            graphics.DrawString(ticket.User.FullName, normalFont, brush, new PointF(10, currentY));

            currentY += padding;
            graphics.DrawString("Date:", subtitleFont, brush, new PointF(10, currentY));
            currentY += padding;
            graphics.DrawString(DateTime.Now.ToString("dd/MM/yyyy HH:mm"), normalFont, brush, new PointF(10, currentY));

            string qrCodeText = $"Flight Number: {ticket.Flight.FlightNumber}\n" +
                                $"Departure: {ticket.Flight.DepartureAirport.Name}\n" +
                                $"Arrival: {ticket.Flight.ArrivalAirport.Name}\n" +
                                $"Seat Number: {ticket.SeatNumber}\n" +
                                $"Departure Time: {ticket.Flight.DepartureTime.ToString("dd/MM/yyyy HH:mm")}\n" +
                                $"Arrival Time: {ticket.Flight.ArrivalTime.ToString("dd/MM/yyyy HH:mm")}\n" +
                                $"Passenger: {ticket.User.FullName}";

            PdfQRBarcode qrCode = new PdfQRBarcode();
            qrCode.Text = qrCodeText;
            qrCode.XDimension = 3;

            qrCode.Draw(page, new PointF(10, currentY + padding));

            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            stream.Position = 0;

            document.Close(true);

            return File(stream, "application/pdf", "BoardingPass.pdf");
        }

        [Authorize(Roles = "Customer")] 
        public async Task<IActionResult> UserTickets()
        {
            var user = await _userRepository.GetUserByEmailAsync(User.Identity.Name);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var tickets = await _flightRepository.GetTicketsByUserIdAsync(user.Id);

            return View(tickets);
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> AllTickets()
        {
            var tickets = await _flightRepository.GetAllTicketsWithAllInfoAsync();

            return View(tickets);
        }

        [Authorize(Roles = "Employee")]
        public IActionResult FlightCalendar()
        {
            return View();
        }

        [Authorize(Roles = "Employee")]
        public JsonResult GetFlights()
        {
            var flights = _flightRepository.GetAllFlights()
                .Select(f => new
                {
                    number = f.FlightNumber,
                    title = f.Airplane.Manufacturer + " - " + f.Airplane.Model,
                    start = f.DepartureTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    end = f.ArrivalTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    from = f.DepartureAirport.Name,
                    to = f.ArrivalAirport.Name
                })
                .ToList();

            return new JsonResult(flights);
        }
    }
}