using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.ValueObject;
using PassengersAggregate = GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Aggregate.Passengers;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Application.UseCases;

public sealed class GetPassengerUseCase
{
    private readonly IPassengerRepository _repository;
    public GetPassengerUseCase(IPassengerRepository repository) => _repository = repository;

    public async Task<PassengersAggregate?> ExecuteById(int id) 
        => await _repository.GetByIdAsync(PassengersId.Create(id));

    public async Task<IEnumerable<PassengersAggregate>> ExecuteAll() 
        => await _repository.GetAllAsync();
}