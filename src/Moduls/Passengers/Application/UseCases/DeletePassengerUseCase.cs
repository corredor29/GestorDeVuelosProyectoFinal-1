using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Passengers.Application.UseCases;

public sealed class DeletePassengerUseCase
{
    private readonly IPassengerRepository _repository;
    public DeletePassengerUseCase(IPassengerRepository repository) => _repository = repository;

    public async Task Execute(int id) => await _repository.DeleteAsync(PassengersId.Create(id));
}