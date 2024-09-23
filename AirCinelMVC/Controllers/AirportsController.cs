using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirCinelMVC.Data;
using AirCinelMVC.Data.Entities;
using AirCinelMVC.Models;
using AirCinelMVC.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AirCinelMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AirportsController : Controller
    {
        private readonly IAirportRepository _airportRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public AirportsController(
            IAirportRepository airportRepository, 
            ICountryRepository countryRepository,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper)
        {
            _airportRepository = airportRepository;
            _countryRepository = countryRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Airports
        public IActionResult Index()
        {
            return View(_airportRepository.GetAllAirports().OrderBy(a => a.Name));
        }

        // GET: Airports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AirportNotFound");
            }

            var airport = await _airportRepository.GetByIdAsync(id.Value);
            if (airport == null)
            {
                return new NotFoundViewResult("AirportNotFound");
            }



            return View(airport);
        }

        // GET: Airports/Create
        public IActionResult Create()
        {
            var model = new CreateNewAirportViewModel
            {
                Countries = _countryRepository.GetComboCountries(),
                Cities = _countryRepository.GetComboCities(0)
            };

            return View(model);
        }

        // POST: Airports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNewAirportViewModel createNewAirportViewModel)
        {
            if (ModelState.IsValid)
            {        
                Guid imageId = Guid.Empty;

                if (createNewAirportViewModel.ImageFile != null && createNewAirportViewModel.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(createNewAirportViewModel.ImageFile, "flags");
                }

                var newAirport = _converterHelper.ToAirport(createNewAirportViewModel, imageId, true);

                await _airportRepository.CreateAsync(newAirport);
                return RedirectToAction(nameof(Index));
            }

            return View(createNewAirportViewModel);
        }

        // GET: Airports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AirportNotFound");
            }

            var airport = await _airportRepository.GetByIdAsync(id.Value);
            if (airport == null)
            {
                return new NotFoundViewResult("AirportNotFound");
            }



            var createNewAirportViewModel = _converterHelper.ToCreateNewAirportViewModel(airport);

            createNewAirportViewModel.Countries = _countryRepository.GetComboCountries();
            createNewAirportViewModel.Cities = _countryRepository.GetComboCities(createNewAirportViewModel.CountryId);

            return View(createNewAirportViewModel);
        }

        // POST: Airports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateNewAirportViewModel createNewAirportViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = createNewAirportViewModel.ImageId;

                    if (createNewAirportViewModel.ImageFile != null && createNewAirportViewModel.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(createNewAirportViewModel.ImageFile, "flags");
                    }

                    var airport = _converterHelper.ToAirport(createNewAirportViewModel, imageId, false);

                    await _airportRepository.UpdateAsync(airport);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _airportRepository.ExistAsync(createNewAirportViewModel.Id))
                    {
                        return new NotFoundViewResult("AirportNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(createNewAirportViewModel);
        }

        // GET: Airports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return new NotFoundViewResult("AirportNotFound");
            }

            var airport = await _airportRepository.GetByIdAsync(id.Value);
            if (airport == null)
            {
                return new NotFoundViewResult("AirportNotFound");
            }

            try
            {
                await _airportRepository.DeleteAsync(airport);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{airport.Name} is probably being used!!";
                    ViewBag.ErrorMessage = $"{airport.Name} can't be deleted because there are flights that use it.</br></br>" +
                                           $"If you want to delete this airport, please remove the flights that are using it," +
                                           $"and then try to delete it again.";
                }

                return View("Error");
            }
        }

        public IActionResult AirportNotFound()
        {
            return View();
        }

        [HttpPost]
        [Route("Airports/GetCitiesAsync")]
        public async Task<JsonResult> GetCitiesAsync(int countryId)
        {
            var country = await _countryRepository.GetCountryWithCitiesAsync(countryId);
            
            var cities = country.Cities
                .OrderBy(c => c.Name)
                .Select(c => new {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

            return Json(cities);
        }
    }
}
