using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Application.UseCases;

public sealed class GetAvailabilityStatesUseCase
{
    private readonly IAvailabilityStatesRepository _repository;

    public GetAvailabilityStatesUseCase(IAvailabilityStatesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<AvailabilityState>> ExecuteAllAsync() => _repository.GetAllAsync();
    public Task<AvailabilityState?> ExecuteByIdAsync(int id) => _repository.GetByIdAsync(AvailabilityStatesId.Create(id));
    public Task<AvailabilityState?> ExecuteByNameAsync(string name) => _repository.GetByNameAsync(AvailabilityStatesName.Create(name));
    public Task<IEnumerable<AvailabilityState>> ExecuteByStaffIdAsync(int staffId) => _repository.GetByStaffIdAsync(PersonalId.Create(staffId));
}
