using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Routes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Routes.Application.UseCases;

public sealed class GetRoutesUseCase
{
    private readonly IRoutesRepository _repository;

    public GetRoutesUseCase(IRoutesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Route>> ExecuteAllAsync() => _repository.GetAllAsync();
    public Task<Route?> ExecuteByIdAsync(int id) => _repository.GetByIdAsync(RouteId.Create(id));
    public Task<IEnumerable<Route>> ExecuteByOriginAirportIdAsync(int originAirportId) => _repository.GetByOriginAirportIdAsync(AirportsId.Create(originAirportId));
    public Task<IEnumerable<Route>> ExecuteByDestinationAirportIdAsync(int destinationAirportId) => _repository.GetByDestinationAirportIdAsync(AirportsId.Create(destinationAirportId));
    public Task<Route?> ExecuteByOriginAndDestinationAsync(int originAirportId, int destinationAirportId) => _repository.GetByOriginAndDestinationAsync(AirportsId.Create(originAirportId), AirportsId.Create(destinationAirportId));
}
