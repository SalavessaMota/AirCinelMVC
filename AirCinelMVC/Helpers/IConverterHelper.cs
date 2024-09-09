using AirCinelMVC.Data.Entities;
using AirCinelMVC.Models;

namespace AirCinelMVC.Helpers
{
    public interface IConverterHelper
    {
        Airplane ToAirplane(AirplaneViewModel model, string path, bool isNew); 

        AirplaneViewModel ToAirplaneViewModel(Airplane airplane);
    }
}
