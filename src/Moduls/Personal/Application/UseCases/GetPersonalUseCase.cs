using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Airports.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.People.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.PersonalPositions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Personal.Application.UseCases;

public sealed class GetPersonalUseCase
{
    private readonly IPersonalRepository _repository;

    public GetPersonalUseCase(IPersonalRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Staff>> ExecuteAllAsync() => _repository.GetAllAsync();
    public Task<Staff?> ExecuteByIdAsync(int id) => _repository.GetByIdAsync(PersonalId.Create(id));
    public Task<Staff?> ExecuteByPersonIdAsync(int personId) => _repository.GetByPersonIdAsync(PeopleId.Create(personId));
    public Task<IEnumerable<Staff>> ExecuteByPersonNameAsync(string personName) => _repository.GetByPersonNameAsync(personName);
    public Task<IEnumerable<Staff>> ExecuteByPositionIdAsync(int positionId) => _repository.GetByPositionIdAsync(PersonalPositionsId.Create(positionId));
    public Task<IEnumerable<Staff>> ExecuteByPositionNameAsync(string positionName) => _repository.GetByPositionNameAsync(positionName);
    public Task<IEnumerable<Staff>> ExecuteByAirlineIdAsync(int airlineId) => _repository.GetByAirlineIdAsync(AirlinesId.Create(airlineId));
    public Task<IEnumerable<Staff>> ExecuteByAirlineNameAsync(string airlineName) => _repository.GetByAirlineNameAsync(airlineName);
    public Task<IEnumerable<Staff>> ExecuteByAirportIdAsync(int airportId) => _repository.GetByAirportIdAsync(AirportsId.Create(airportId));
    public Task<IEnumerable<Staff>> ExecuteByAirportNameAsync(string airportName) => _repository.GetByAirportNameAsync(airportName);
    public Task<IEnumerable<Staff>> ExecuteByIsActiveAsync(bool isActive) => _repository.GetByIsActiveAsync(isActive);
}
