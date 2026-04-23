using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Application.UseCases;

public sealed class QueryPassengerTypesUseCase
{
    private readonly IPassengerTypesRepository _repository;

    public QueryPassengerTypesUseCase(IPassengerTypesRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<PassengerType>> GetAllAsync(CancellationToken cancellationToken = default)
        => _repository.GetAllAsync(cancellationToken);

    public async Task<PassengerType?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _repository.GetByIdAsync(PassengerTypesId.Create(id), cancellationToken);

    public Task<PassengerType?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        => _repository.GetByNameAsync(PassengerTypesName.Create(name), cancellationToken);

    public Task<PassengerType?> GetByAgeAsync(int ageInYears, CancellationToken cancellationToken = default)
        => _repository.GetByAgeAsync(ageInYears, cancellationToken);
}
