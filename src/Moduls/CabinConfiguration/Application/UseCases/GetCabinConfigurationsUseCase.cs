using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.ValueObject;
using CabinConfigurationAggregate = GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Aggregate.CabinConfiguration;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Application.UseCases;

public sealed class GetCabinConfigurationsUseCase
{
    private readonly ICabinConfigurationRepository _repository;

    public GetCabinConfigurationsUseCase(ICabinConfigurationRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<CabinConfigurationAggregate>> ExecuteAsync()
        => _repository.GetAllAsync();

    public Task<IEnumerable<CabinConfigurationAggregate>> GetByAircraftIdAsync(int aircraftId)
        => _repository.GetByAircraftIdAsync(aircraftId);

    public Task<CabinConfigurationAggregate?> GetByIdAsync(int id)
        => _repository.GetByIdAsync(CabinConfigurationId.Create(id));
}
