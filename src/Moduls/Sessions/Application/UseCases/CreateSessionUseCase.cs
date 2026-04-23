using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Repositories;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Application.UseCases;

public sealed class CreateSessionUseCase
{
    private readonly ISessionsRepository _repository;

    public CreateSessionUseCase(ISessionsRepository repository)
    {
        _repository = repository;
    }

    public async Task<Session> ExecuteAsync(
        int id,
        int userId,
        string? ipAddress,
        CancellationToken cancellationToken = default)
    {
        var session = Session.Create(id, userId, ipAddress, DateTime.UtcNow);

        await _repository.SaveAsync(session);

        return session;
    }
}
