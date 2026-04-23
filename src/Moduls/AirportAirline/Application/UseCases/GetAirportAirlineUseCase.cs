using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.AirportAirline.Application.UseCases;

public sealed class GetAirportAirlineUseCase
{
    private readonly IAirportAirlineRepository _repository;

    public GetAirportAirlineUseCase(IAirportAirlineRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<AirportAirlineOperation>> ExecuteAllAsync() => _repository.GetAllAsync();
    public Task<IEnumerable<AirportAirlineOperation>> ExecuteActiveAsync() => _repository.GetActiveAsync();
    public Task<AirportAirlineOperation?> ExecuteByIdAsync(int id) => _repository.GetByIdAsync(AirportAirlineId.Create(id));
    public Task<IEnumerable<AirportAirlineOperation>> ExecuteByTerminalAsync(string terminal) => _repository.GetByTerminalAsync(AirportAirlineTerminal.Create(terminal));
    public Task<IEnumerable<AirportAirlineOperation>> ExecuteByAirportIdAsync(int airportId) => _repository.GetByAirportIdAsync(AirportsId.Create(airportId));
    public Task<IEnumerable<AirportAirlineOperation>> ExecuteByAirlineIdAsync(int airlineId) => _repository.GetByAirlineIdAsync(AirlinesId.Create(airlineId));
    public Task<IEnumerable<AirportAirlineOperation>> ExecuteByStartDateAsync(DateTime startDate) => _repository.GetByStartDateAsync(AirportAirlineStartDate.Create(startDate));
    public Task<IEnumerable<AirportAirlineOperation>> ExecuteByEndDateAsync(DateTime endDate) => _repository.GetByEndDateAsync(AirportAirlineEndDate.Create(endDate));
}
