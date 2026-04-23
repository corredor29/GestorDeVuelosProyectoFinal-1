using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.Airlines.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Countries.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.Airlines.Application.UseCases;

public sealed class GetAirlinesUseCase
{
    private readonly IAirlinesRepository _repository;

    public GetAirlinesUseCase(IAirlinesRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Airline>> ExecuteAllAsync() => _repository.GetAllAsync();
    public Task<IEnumerable<Airline>> ExecuteActiveAsync() => _repository.GetActiveAsync();
    public Task<Airline?> ExecuteByIdAsync(int id) => _repository.GetByIdAsync(AirlinesId.Create(id));
    public Task<Airline?> ExecuteByNameAsync(string name) => _repository.GetByNameAsync(AirlinesName.Create(name));
    public Task<Airline?> ExecuteByIataCodeAsync(string iataCode) => _repository.GetByIataCodeAsync(AirlinesIataCode.Create(iataCode));
    public Task<IEnumerable<Airline>> ExecuteByOriginCountryIdAsync(int originCountryId) => _repository.GetByOriginCountryIdAsync(CountryId.Create(originCountryId));
}
