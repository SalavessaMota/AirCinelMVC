using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AirCinelMVC.Data;
using AirCinelMVC.Data.Entities;
using AirCinelMVC.Helpers;
using AirCinelMVC.Models;
using System.IO;
using System;


namespace AirCinelMVC.Controllers
{
    public class AirplanesController : Controller
    {
        private readonly IAirplaneRepository _airplaneRepository;
        private readonly IUserHelper _userHelper;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public AirplanesController(
            IAirplaneRepository airplaneRepository, 
            IUserHelper userHelper,
            IImageHelper imageHelper,
            IConverterHelper converterHelper)
        {
            _airplaneRepository = airplaneRepository;
            _userHelper = userHelper;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: Airplanes
        public async Task<IActionResult> Index()
        {
            return View(_airplaneRepository.GetAll().OrderBy(a => a.Model));
        }

        // GET: Airplanes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);
            if (airplane == null)
            {
                return NotFound();
            }

            return View(airplane);
        }

        // GET: Airplanes/Create
        public IActionResult Create()
        {
            return View();
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
                var path = string.Empty;

                if (airplaneViewModel.ImageFile != null && airplaneViewModel.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(airplaneViewModel.ImageFile, "airplanes");
                }

                var airplane = _converterHelper.ToAirplane(airplaneViewModel, path, true);

                //TODO: "Change to the logged user"
                airplane.User = await _userHelper.GetUserByEmailAsync("nunosalavessa@hotmail.com");
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
                return NotFound();
            }

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);
            if (airplane == null)
            {
                return NotFound();
            }

            var airplaneViewModel = _converterHelper.ToAirplaneViewModel(airplane);

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
                    var path = airplaneViewModel.ImageUrl;

                    if(airplaneViewModel.ImageFile != null && airplaneViewModel.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(airplaneViewModel.ImageFile, "airplanes");
                    }

                    var airplane = _converterHelper.ToAirplane(airplaneViewModel, path, false);

                    //TODO: "Change to the logged user"
                    airplane.User = await _userHelper.GetUserByEmailAsync("nunosalavessa@hotmail.com");
                    await _airplaneRepository.UpdateAsync(airplane);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _airplaneRepository.ExistAsync(airplaneViewModel.Id))
                    {
                        return NotFound();
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
                return NotFound();
            }

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);
            if (airplane == null)
            {
                return NotFound();
            }

            return View(airplane);
        }

        // POST: Airplanes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airplane = await _airplaneRepository.GetByIdAsync(id);
            await _airplaneRepository.DeleteAsync(airplane);
            return RedirectToAction(nameof(Index));
        }
    }
}
