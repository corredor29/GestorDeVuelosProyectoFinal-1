using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Application.Interfaces;
using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Contracts;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Application.Services;

public sealed class SessionsService : ISessionsService
{
    private readonly ISessionsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SessionsService(
        ISessionsRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Session> CreateAsync(
        int id,
        int userId,
        string? ipAddress,
        CancellationToken cancellationToken = default)
    {
        var session = Session.Create(id, userId, ipAddress, DateTime.UtcNow);

        await _repository.SaveAsync(session);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return session;
    }

    public async Task<Session?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByIdAsync(SessionsId.Create(id));
    }

    public Task<IEnumerable<Session>> GetActiveSessionsByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return _repository.GetActiveSessionsByUserIdAsync(UsersId.Create(userId));
    }

    public async Task<bool> CloseSessionAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetByIdAsync(SessionsId.Create(id));
        if (existing is null)
            return false;

        existing.CloseSession();

        await _repository.UpdateAsync(existing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}