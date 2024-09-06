using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AirCinelMVC.Data;
using AirCinelMVC.Data.Entities;

namespace AirCinelMVC.Controllers
{
    public class AirplanesController : Controller
    {
        private readonly IAirplaneRepository _airplaneRepository;

        public AirplanesController(IAirplaneRepository airplaneRepository)
        {
            _airplaneRepository = airplaneRepository;
        }

        // GET: Airplanes
        public async Task<IActionResult> Index()
        {
            return View(_airplaneRepository.GetAll());
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
        public async Task<IActionResult> Create(Airplane airplane)
        {
            if (ModelState.IsValid)
            {
                await _airplaneRepository.CreateAsync(airplane);                
                return RedirectToAction(nameof(Index));
            }
            return View(airplane);
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
            return View(airplane);
        }

        // POST: Airplanes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Airplane airplane)
        {
            if (id != airplane.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _airplaneRepository.UpdateAsync(airplane);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _airplaneRepository.ExistAsync(airplane.Id))
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
            return View(airplane);
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
