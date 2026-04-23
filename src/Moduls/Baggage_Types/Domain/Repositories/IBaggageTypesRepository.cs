using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.BaggageTypes.Domain.Repositories;

public interface IBaggageTypesRepository
{
    Task<BaggageType?> GetByIdAsync(BaggageTypeId id);
    Task<BaggageType?> GetByNameAsync(BaggageTypeName name);
    Task<IEnumerable<BaggageType>> GetAllAsync();
    Task SaveAsync(BaggageType baggageType);
    Task UpdateAsync(BaggageType baggageType);
    Task DeleteAsync(BaggageTypeId id);
}