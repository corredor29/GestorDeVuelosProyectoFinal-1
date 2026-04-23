using PassengersAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Aggregate.Passengers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Application.Interfaces;

public interface IPassengerService
{
    Task CreatePassenger(int personId, int passengerTypeId);
    Task<PassengersAggregate?> GetPassengerById(int id);
    Task<IEnumerable<PassengersAggregate>> GetAllPassengers();
    Task DeletePassenger(int id);
}