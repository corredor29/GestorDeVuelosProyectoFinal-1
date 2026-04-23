using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Checkins.Application.UseCases;

public sealed class UpdateCheckinUseCase
{
    private readonly ICheckinsRepository _repository;

    public UpdateCheckinUseCase(ICheckinsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Checkin> ExecuteAsync(
        int id,
        int? newCheckinStatusId,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(CheckinsId.Create(id));
        if (existing is null)
            throw new KeyNotFoundException($"Checkin with id '{id}' was not found.");

        if (newCheckinStatusId is not null)
            existing.UpdateCheckinStatus(newCheckinStatusId.Value);

        await _repository.UpdateAsync(existing);

        return existing;
    }
}