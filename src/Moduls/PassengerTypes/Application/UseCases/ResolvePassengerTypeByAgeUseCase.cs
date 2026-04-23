using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Application;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.Moduls.PassengerTypes.Application.UseCases;

public sealed class ResolvePassengerTypeByAgeUseCase
{
    private readonly IPassengerTypesRepository _repository;

    public ResolvePassengerTypeByAgeUseCase(IPassengerTypesRepository repository)
    {
        _repository = repository;
    }

    public async Task<PassengerType?> ExecuteAsync(
        DateTime birthDate,
        DateTime? referenceDateUtc = null,
        CancellationToken cancellationToken = default)
    {
        var reference = referenceDateUtc?.Date ?? DateTime.UtcNow.Date;
        var age = PassengerAgeCalculator.ComputeAgeInWholeYears(birthDate, reference);
        return await _repository.GetByAgeAsync(age, cancellationToken);
    }
}
