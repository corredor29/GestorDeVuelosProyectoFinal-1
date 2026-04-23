using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Aggregate;
namespace GestorDeVuelosProyectoFinal.src.Moduls.Baggage.Domain.Repositories;

public interface IBaggageRepository
{
    Task<BaggageRoot?> FindByIdAsync(BaggageId id);
    Task<IEnumerable<BaggageRoot>> FindAllAsync();
    Task<IEnumerable<BaggageRoot>> FindByCheckinIdAsync(int checkinId);
    Task SaveAsync(BaggageRoot baggage);
    Task UpdateAsync(BaggageRoot baggage);
    Task DeleteAsync(BaggageId id);
}