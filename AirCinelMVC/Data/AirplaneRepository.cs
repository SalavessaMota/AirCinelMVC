﻿using AirCinelMVC.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<SelectListItem>> GetComboModelsAsync(int manufacturerId)
        {
            var manufacturer = await _context.Manufacturers.FindAsync(manufacturerId);
            var list = new List<SelectListItem>();

            if (manufacturer != null)
            {
                list = manufacturer.Models.Select(m => new SelectListItem
                {
                    Text = m.Name,
                    Value = m.Id.ToString()
                })
                .OrderBy(l => l.Text)
                .ToList();

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

        public int GetManufacturerIdByName(string name)
        {
            return _context.Manufacturers.FirstOrDefault(m => m.Name == name).Id;
        }

        public int GetModelIdByName(string name)
        {
            return _context.Models.FirstOrDefault(m => m.Name == name).Id;
        }
    }
}