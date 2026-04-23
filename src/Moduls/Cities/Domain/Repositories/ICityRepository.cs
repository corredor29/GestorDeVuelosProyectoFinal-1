using System.Collections.Generic;
using System.Threading.Tasks;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Regions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Cities.Domain.Repositories;

public interface ICityRepository
{
    Task<City?> GetByIdAsync(CityId id);
    Task<City?> GetByNameAsync(string name);
    Task<IEnumerable<City>> GetAllAsync();
    Task<IEnumerable<City>> GetByCountryAsync(RegionId regionId);
    Task saveAsync(City city);
    Task SaveAsync(City city);
    Task DeleteAsync(CityId id);
    Task DeleteByNameAsync(string name);
    Task DeleteByCountryAsync(RegionId regionId);
}