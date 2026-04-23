using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Application.UseCases;

public sealed class GetCheckinUseCase
{
    private readonly ICheckinsRepository _repository;

    public GetCheckinUseCase(ICheckinsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Checkin> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var result = await _repository.GetByIdAsync(CheckinsId.Create(id));
        if (result is null)
            throw new KeyNotFoundException($"Checkin with id '{id}' was not found.");

        return result;
    }
}