using AirCinelMVC.Data.Entities;
using AirCinelMVC.Models;
using System;

namespace AirCinelMVC.Helpers
{
    public interface IConverterHelper
    {
        Airplane ToAirplane(AirplaneViewModel model, Guid imageId, bool isNew); 

        AirplaneViewModel ToAirplaneViewModel(Airplane airplane);
    }
}
