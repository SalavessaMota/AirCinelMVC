﻿using AirCinelMVC.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AirCinelMVC.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : Controller
    {
        
        private readonly ICountryRepository _countryRepository;

        public CountriesController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [HttpGet("countries")]
        public IActionResult GetCountries()
        {
            var countries = _countryRepository.GetComboCountries()
                .Select(c => new { Id = int.Parse(c.Value), Name = c.Text })
                .ToList();

            return Ok(countries);
        }

        [HttpGet("cities/{countryId}")]
        public async Task<IActionResult> GetCities(int countryId)
        {
            var cities = await _countryRepository.GetComboCitiesAsync(countryId);
            var cityList = cities
                .Select(c => new { Id = int.Parse(c.Value), Name = c.Text })
                .Where(c => c.Id > 0)
                .ToList();

            return Ok(cityList);
        }
    }
}