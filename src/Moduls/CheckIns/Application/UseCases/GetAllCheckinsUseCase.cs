using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Application.UseCases;

public sealed class GetAllCheckinsUseCase
{
    private readonly ICheckinsRepository _repository;

    public GetAllCheckinsUseCase(ICheckinsRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Checkin>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return _repository.GetAllAsync();
    }
}