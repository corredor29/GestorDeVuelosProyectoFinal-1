using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Application.UseCases;
using PassengersAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Aggregate.Passengers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Application.Services;

public sealed class PassengerService : IPassengerService
{
    private readonly CreatePassengerUseCase _createUseCase;
    private readonly GetPassengerUseCase _getUseCase;
    private readonly DeletePassengerUseCase _deleteUseCase;

    public PassengerService(
        CreatePassengerUseCase createUseCase,
        GetPassengerUseCase getUseCase,
        DeletePassengerUseCase deleteUseCase)
    {
        _createUseCase = createUseCase;
        _getUseCase = getUseCase;
        _deleteUseCase = deleteUseCase;
    }

    public Task CreatePassenger(int personId, int passengerTypeId) 
        => _createUseCase.Execute(personId, passengerTypeId);

    public Task<PassengersAggregate?> GetPassengerById(int id) 
        => _getUseCase.ExecuteById(id);

    public Task<IEnumerable<PassengersAggregate>> GetAllPassengers() 
        => _getUseCase.ExecuteAll();

    public Task DeletePassenger(int id) 
        => _deleteUseCase.Execute(id);
}