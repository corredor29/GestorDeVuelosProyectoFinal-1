using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Application.UseCases;

public sealed class CloseSessionUseCase
{
    private readonly ISessionsRepository _repository;

    public CloseSessionUseCase(ISessionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var sessionId = SessionsId.Create(id);

        var existing = await _repository.GetByIdAsync(sessionId);
        if (existing is null)
            return false;

        existing.CloseSession();

        await _repository.UpdateAsync(existing);

        return true;
    }
}