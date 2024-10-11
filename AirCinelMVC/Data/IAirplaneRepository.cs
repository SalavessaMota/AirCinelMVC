using AirCinelMVC.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirCinelMVC.Data
{
    public interface IAirplaneRepository : IGenericRepository<Airplane>
    {
        IQueryable<Airplane> GetAllAirplanes();

        IEnumerable<SelectListItem> GetComboManufacturers();

        Task<IEnumerable<SelectListItem>> GetComboModelsAsync(int manufacturerId);

        string GetManufacturerNameById(int manufacturerId);

        Task<Manufacturer> GetManufacturerWithModelsAsync(int id);

        string GetModelNameById(int modelId);

        int GetManufacturerIdByName(string name);

        int GetModelIdByName(string name);
    }
}