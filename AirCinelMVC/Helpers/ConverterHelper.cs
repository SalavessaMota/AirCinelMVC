using System;
using AirCinelMVC.Data;
using AirCinelMVC.Data.Entities;
using AirCinelMVC.Models;

namespace AirCinelMVC.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly IAirplaneRepository _airplaneRepository;
        private readonly ICountryRepository _countryRepository;

        public ConverterHelper(
            IAirplaneRepository airplaneRepository,
            ICountryRepository countryRepository)
        {
            _airplaneRepository = airplaneRepository;
            _countryRepository = countryRepository;
        }

        public Airplane ToAirplane(AirplaneViewModel model, Guid imageId, bool isNew)
        {
            return new Airplane
            {
                Id = isNew ? 0 : model.Id,
                Model = _airplaneRepository.GetModelNameById(model.ModelId),
                Manufacturer = _airplaneRepository.GetManufacturerNameById(model.ManufacturerId),
                Capacity = model.Capacity,
                YearOfManufacture = model.YearOfManufacture,
                ImageId = imageId,
                ModelId = model.ModelId
            };
        }

        public AirplaneViewModel ToAirplaneViewModel(Airplane airplane)
        {
            return new AirplaneViewModel
            {
                Id = airplane.Id,
                Model = airplane.Model,
                Manufacturer = airplane.Manufacturer,
                Capacity = airplane.Capacity,
                YearOfManufacture = airplane.YearOfManufacture,
                ImageId = airplane.ImageId,
                ManufacturerId = _airplaneRepository.GetManufacturerIdByName(airplane.Manufacturer),
                ModelId = _airplaneRepository.GetModelIdByName(airplane.Model)
            };
        }


        public Airport ToAirport(CreateNewAirportViewModel model, Guid imageId, bool isNew)
        {
            return new Airport
            {
                Id = isNew ? 0 : model.Id,
                Name = model.Name,
                CityId = model.CityId,
                ImageId = imageId
            };
        }

        public CreateNewAirportViewModel ToCreateNewAirportViewModel(Airport airport)
        {
            var city = _countryRepository.GetCityAsync(airport.CityId).Result;
            var country = _countryRepository.GetCountryAsync(city).Result;

            return new CreateNewAirportViewModel
            {
                Id = airport.Id,
                Name = airport.Name,
                CityId = airport.CityId,
                ImageId = airport.ImageId,
                CountryId = country.Id
            };
        }
    }
}
