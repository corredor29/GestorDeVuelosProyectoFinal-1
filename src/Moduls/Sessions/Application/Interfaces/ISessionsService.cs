using GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Sessions.Application.Interfaces;

public interface ISessionsService
{
    Task<Session> CreateAsync(int id, int userId, string? ipAddress, CancellationToken cancellationToken = default);
    Task<Session?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Session>> GetActiveSessionsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<bool> CloseSessionAsync(int id, CancellationToken cancellationToken = default);
}