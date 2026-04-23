using System;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Repositories;

public interface IPassengerRepository
{
    Task<Aggregate.Passengers?> GetByIdAsync(PassengersId id);
    Task<Aggregate.Passengers?> GetByPersonIdAsync(PassengersPersonId personId); 
    Task<IEnumerable<Aggregate.Passengers>> GetAllAsync();
    Task SaveAsync(Aggregate.Passengers passengers);
    Task DeleteAsync(PassengersId id);
}