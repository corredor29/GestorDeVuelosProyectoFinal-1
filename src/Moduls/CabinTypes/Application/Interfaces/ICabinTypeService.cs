using GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinTypes.Application.Interfaces;

public interface ICabinTypeService
{
    Task CreateAsync(string name);
    Task<IEnumerable<CabinType>> GetAllAsync();
    Task UpdateAsync(int id, string name);
    Task DeleteByIdAsync(int id);
    Task DeleteByNameAsync(string name);
}
