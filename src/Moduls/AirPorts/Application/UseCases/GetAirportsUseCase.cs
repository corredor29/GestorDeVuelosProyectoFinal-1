using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Airports.Application.UseCases;

public sealed class GetAirportsUseCase
{
    private readonly IAirportsRepository _repository;

    public GetAirportsUseCase(IAirportsRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Airport>> ExecuteAllAsync() => _repository.GetAllAsync();
    public Task<Airport?> ExecuteByIdAsync(int id) => _repository.GetByIdAsync(AirportsId.Create(id));
    public Task<Airport?> ExecuteByNameAsync(string name) => _repository.GetByNameAsync(AirportsName.Create(name));
    public Task<Airport?> ExecuteByIataCodeAsync(string iataCode) => _repository.GetByIataCodeAsync(AirportsIataCode.Create(iataCode));
    public Task<Airport?> ExecuteByIcaoCodeAsync(string? icaoCode) => _repository.GetByIcaoCodeAsync(AirportsIcaoCode.Create(icaoCode));
    public Task<IEnumerable<Airport>> ExecuteByCityIdAsync(int cityId) => _repository.GetByCityIdAsync(CityId.Create(cityId));
    public Task<IEnumerable<Airport>> ExecuteByCityNameAsync(string cityName) => _repository.GetByCityNameAsync(cityName);
}
