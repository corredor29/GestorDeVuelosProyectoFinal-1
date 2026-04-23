using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.ValueObject;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Application.UseCases;

public sealed class GetSessionUseCase
{
    private readonly ISessionsRepository _repository;

    public GetSessionUseCase(ISessionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Session> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var sessionId = SessionsId.Create(id);

        var result = await _repository.GetByIdAsync(sessionId);
        if (result is null)
            throw new KeyNotFoundException($"Session with id '{id}' was not found.");

        return result;
    }
}