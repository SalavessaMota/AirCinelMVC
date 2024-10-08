using AirCinelMVC.Data;
using AirCinelMVC.Helpers;
using AirCinelMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AirCinelMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AirplanesController : Controller
    {
        private readonly IAirplaneRepository _airplaneRepository;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;

        public AirplanesController(
            IAirplaneRepository airplaneRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper)
        {
            _airplaneRepository = airplaneRepository;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Airplanes
        public IActionResult Index()
        {
            return View(_airplaneRepository.GetAllAirplanes().OrderBy(a => a.Manufacturer).ThenBy(a => a.Model));
        }

        // GET: Airplanes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AirplaneNotFound");
            }

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);
            if (airplane == null)
            {
                return new NotFoundViewResult("AirplaneNotFound");
            }

            return View(airplane);
        }

        // GET: Airplanes/Create
        public IActionResult Create()
        {
            var model = new AirplaneViewModel
            {
                Manufacturers = _airplaneRepository.GetComboManufacturers(),
                Models = _airplaneRepository.GetComboModels(0)
            };

            return View(model);
        }

        // POST: Airplanes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AirplaneViewModel airplaneViewModel)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (airplaneViewModel.ImageFile != null && airplaneViewModel.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(airplaneViewModel.ImageFile, "airplanes");
                }

                var airplane = _converterHelper.ToAirplane(airplaneViewModel, imageId, true);

                //TODO: "Change to the logged user"
                //airplane.User = await _userHelper.GetUserByEmailAsync("nunosalavessa@hotmail.com");
                await _airplaneRepository.CreateAsync(airplane);

                return RedirectToAction(nameof(Index));
            }

            return View(airplaneViewModel);
        }

        // GET: Airplanes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AirplaneNotFound");
            }

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);
            if (airplane == null)
            {
                return new NotFoundViewResult("AirplaneNotFound");
            }

            var airplaneViewModel = _converterHelper.ToAirplaneViewModel(airplane);

            airplaneViewModel.Manufacturers = _airplaneRepository.GetComboManufacturers();
            airplaneViewModel.Models = _airplaneRepository.GetComboModels(airplaneViewModel.ManufacturerId);

            return View(airplaneViewModel);
        }

        // POST: Airplanes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AirplaneViewModel airplaneViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = airplaneViewModel.ImageId;

                    if (airplaneViewModel.ImageFile != null && airplaneViewModel.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(airplaneViewModel.ImageFile, "airplanes");
                    }

                    var airplane = _converterHelper.ToAirplane(airplaneViewModel, imageId, false);

                    //TODO: "Change to the logged user"
                    //airplane.User = await _userHelper.GetUserByEmailAsync("nunosalavessa@hotmail.com");
                    await _airplaneRepository.UpdateAsync(airplane);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _airplaneRepository.ExistAsync(airplaneViewModel.Id))
                    {
                        return new NotFoundViewResult("AirplaneNotFound");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(airplaneViewModel);
        }

        // GET: Airplanes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AirplaneNotFound");
            }

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);
            if (airplane == null)
            {
                return new NotFoundViewResult("AirplaneNotFound");
            }

            try
            {
                await _airplaneRepository.DeleteAsync(airplane);
                await _blobHelper.DeleteBlobAsync("airplanes", airplane.ImageId.ToString());
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"This {airplane.Manufacturer} {airplane.Model} is probably being used!!";
                    ViewBag.ErrorMessage = $"{airplane.Manufacturer} {airplane.Model}  can't be deleted because there are flights that use it.</br></br>" +
                                           $"If you want to delete this airplane, please remove the flights that are using it," +
                                           $"and then try to delete it again.";
                }

                return View("Error");
            }
        }

        public IActionResult AirplaneNotFound()
        {
            return View();
        }

        [HttpPost]
        [Route("Airplanes/GetModelsAsync")]
        public async Task<JsonResult> GetModelsAsync(int manufacturerId)
        {
            var manufacturer = await _airplaneRepository.GetManufacturerWithModelsAsync(manufacturerId);
            return Json(manufacturer.Models.OrderBy(m => m.Name));
        }
    }
}