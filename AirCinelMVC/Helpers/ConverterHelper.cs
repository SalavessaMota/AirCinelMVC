using System;
using AirCinelMVC.Data.Entities;
using AirCinelMVC.Models;

namespace AirCinelMVC.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Airplane ToAirplane(AirplaneViewModel model, Guid imageId, bool isNew)
        {
            return new Airplane
            {
                Id = isNew ? 0 : model.Id,
                Model = model.Model,
                Manufacturer = model.Manufacturer,
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
                ImageId = airplane.ImageId
            };
        }
    }
}
