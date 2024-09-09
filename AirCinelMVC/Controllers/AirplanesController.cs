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

        public AirplanesController(
            IAirplaneRepository airplaneRepository, 
            IUserHelper userHelper)
        {
            _airplaneRepository = airplaneRepository;
            _userHelper = userHelper;
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
                    var guid = Guid.NewGuid().ToString();
                    var file = $"{guid}.jpg";

                    path = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\airplanes",
                        file);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await airplaneViewModel.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/airplanes/{file}";
                }

                var airplane = this.ToAirplane(airplaneViewModel, path);

                //TODO: "Change to the logged user"
                airplane.User = await _userHelper.GetUserByEmailAsync("nunosalavessa@hotmail.com");
                await _airplaneRepository.CreateAsync(airplane);
                return RedirectToAction(nameof(Index));
            }

            return View(airplaneViewModel);
        }

        private Airplane ToAirplane(AirplaneViewModel model, string path)
        {
            return new Airplane
            {
                Id = model.Id,
                Model = model.Model,
                Manufacturer = model.Manufacturer,
                Capacity = model.Capacity,
                YearOfManufacture = model.YearOfManufacture,
                ImageUrl = path,
                User = model.User
            };
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

            var airplaneViewModel = this.ToAirplaneViewModel(airplane);

            return View(airplaneViewModel);
        }

        private AirplaneViewModel ToAirplaneViewModel(Airplane airplane)
        {
            return new AirplaneViewModel
            {
                Id = airplane.Id,
                Model = airplane.Model,
                Manufacturer = airplane.Manufacturer,
                Capacity = airplane.Capacity,
                YearOfManufacture = airplane.YearOfManufacture,
                ImageUrl = airplane.ImageUrl,
                User = airplane.User
            };
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
                        var guid = Guid.NewGuid().ToString();
                        var file = $"{guid}.jpg";

                        path = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot\\images\\airplanes",
                            file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await airplaneViewModel.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/airplanes/{file}";
                    }

                    var airplane = this.ToAirplane(airplaneViewModel, path);

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
