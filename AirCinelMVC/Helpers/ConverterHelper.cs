using System;
using AirCinelMVC.Data;
using AirCinelMVC.Data.Entities;
using AirCinelMVC.Models;

namespace AirCinelMVC.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly IAirplaneRepository _airplaneRepository;

        public ConverterHelper(IAirplaneRepository airplaneRepository)
        {
            _airplaneRepository = airplaneRepository;
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
                ImageId = imageId
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
    }
}
