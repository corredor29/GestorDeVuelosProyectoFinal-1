using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.ValueObject;
using PassengersAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Aggregate.Passengers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Application.UseCases;

public sealed class CreatePassengerUseCase
{
    private readonly IPassengerRepository _repository;

    public CreatePassengerUseCase(IPassengerRepository repository)
    {
        _repository = repository;
    }

    public async Task Execute(int personId, int passengerTypeId)
    {
        // Validar si la persona ya existe como pasajero (Regla UNIQUE)
        var existing = await _repository.GetByPersonIdAsync(PassengersPersonId.Create(personId));
        if (existing != null)
            throw new InvalidOperationException("Esta persona ya está registrada como pasajero.");

        var passenger = PassengersAggregate.Create(personId, passengerTypeId);
        await _repository.SaveAsync(passenger);
    }
}