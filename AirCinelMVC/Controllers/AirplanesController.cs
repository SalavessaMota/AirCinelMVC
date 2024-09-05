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
        private readonly IRepository _repository;

        public AirplanesController(IRepository repository)
        {
            _repository = repository;
        }

        // GET: Airplanes
        public async Task<IActionResult> Index()
        {
            return View(_repository.GetAirplanes());
        }

        // GET: Airplanes/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airplane = _repository.GetAirplane(id.Value);
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
                _repository.AddAirplane(airplane);
                await _repository.SaveAllAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(airplane);
        }

        // GET: Airplanes/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airplane = _repository.GetAirplane(id.Value);
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
                    _repository.UpdateAirplane(airplane);
                    await _repository.SaveAllAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.AirplaneExists(airplane.Id))
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
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airplane = _repository.GetAirplane(id.Value);
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
            var airplane = _repository.GetAirplane(id);
            _repository.RemoveAirplane(airplane);
            await _repository.SaveAllAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
