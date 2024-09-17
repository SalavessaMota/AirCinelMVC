using AirCinelMVC.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace AirCinelMVC.Data
{
    public class AirplaneRepository : GenericRepository<Airplane>, IAirplaneRepository
    {
        private readonly DataContext _context;

        public AirplaneRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<Airplane> GetAllAirplanes()
        {
            return _context.Airplanes;
        }

        public IQueryable<Manufacturer> GetAllManufacturers()
        {
            return _context.Manufacturers;
        }

        public IQueryable<Model> GetAllModels()
        {
            return _context.Models;
        }

        

        public async Task<Manufacturer> GetManufacturerWithModelsAsync(int id)
        {
            return await _context.Manufacturers
                .Include(m => m.Models)
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();
        }

        public IEnumerable<SelectListItem> GetComboManufacturers()
        {
            var list = _context.Manufacturers.Select(m => new SelectListItem
            {
                Text = m.Name,
                Value = m.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Select a manufacturer...]",
                Value = "0"
            });

            return list;
        }


        public IEnumerable<SelectListItem> GetComboModels(int manufacturerId)
        {
            var manufacturer = _context.Manufacturers.Find(manufacturerId);
            var list = new List<SelectListItem>();
            if (manufacturer != null)
            {
                list = manufacturer.Models.Select(m => new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id.ToString()
                }).OrderBy(l => l.Text).ToList();

                list.Insert(0, new SelectListItem
                {
                    Text = "[Select a model...]",
                    Value = "0"
                });
            }

            return list;
        }

        public string GetModelNameById(int modelId)
        {
            return _context.Models.Find(modelId).Name;
        }

        public string GetManufacturerNameById(int manufacturerId)
        {
            return _context.Manufacturers.Find(manufacturerId).Name;
        }
    }
}
