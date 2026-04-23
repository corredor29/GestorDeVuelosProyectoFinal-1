using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Application.Interfaces;
using GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Application.UseCases;
using CabinConfigurationAggregate = GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Domain.Aggregate.CabinConfiguration;

namespace GestorDeVuelosProyectoFinal.Moduls.CabinConfiguration.Application.Services;

public sealed class CabinConfigurationService : ICabinConfigurationService
{
    private readonly GetCabinConfigurationsUseCase _get;
    private readonly CreateCabinConfigurationUseCase _create;
    private readonly UpdateCabinConfigurationUseCase _update;
    private readonly DeleteCabinConfigurationUseCase _delete;

    public CabinConfigurationService(
        GetCabinConfigurationsUseCase get,
        CreateCabinConfigurationUseCase create,
        UpdateCabinConfigurationUseCase update,
        DeleteCabinConfigurationUseCase delete)
    {
        _get = get;
        _create = create;
        _update = update;
        _delete = delete;
    }

    public Task<IEnumerable<CabinConfigurationAggregate>> GetAllAsync()
        => _get.ExecuteAsync();

    public Task<CabinConfigurationAggregate?> GetByIdAsync(int id)
        => _get.GetByIdAsync(id);

    public Task<IEnumerable<CabinConfigurationAggregate>> GetByAircraftIdAsync(int aircraftId)
        => _get.GetByAircraftIdAsync(aircraftId);

    public Task CreateAsync(int aircraftId, int cabinTypeId, int rowStart, int rowEnd, int seatsPerRow, string seatLetters)
        => _create.ExecuteAsync(aircraftId, cabinTypeId, rowStart, rowEnd, seatsPerRow, seatLetters);

    public Task UpdateAsync(int id, int aircraftId, int cabinTypeId, int rowStart, int rowEnd, int seatsPerRow, string seatLetters)
        => _update.ExecuteAsync(id, aircraftId, cabinTypeId, rowStart, rowEnd, seatsPerRow, seatLetters);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}
