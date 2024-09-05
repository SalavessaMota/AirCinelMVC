using AirCinelMVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirCinelMVC.Data
{
    public interface IRepository
    {
        void AddAirplane(Airplane airplane);

        bool AirplaneExists(int id);

        Airplane GetAirplane(int id);

        IEnumerable<Airplane> GetAirplanes();

        void RemoveAirplane(Airplane airplane);

        Task<bool> SaveAllAsync();

        void UpdateAirplane(Airplane airplane);
    }
}