using AirCinelMVC.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirCinelMVC.Data
{
    public interface IAirplaneRepository : IGenericRepository<Airplane>
    {
        public IQueryable<Airplane> GetAllAirplanes();

        public IQueryable<Manufacturer> GetAllManufacturers();

        public IQueryable<Model> GetAllModels();

        public IEnumerable<SelectListItem> GetComboManufacturers();

        public IEnumerable<SelectListItem> GetComboModels(int manufacturerId);

        string GetManufacturerNameById(int manufacturerId);

        public Task<Manufacturer> GetManufacturerWithModelsAsync(int id);

        public string GetModelNameById(int modelId);

        public int GetManufacturerIdByName(string name);

        public int GetModelIdByName(string name);
    }
}