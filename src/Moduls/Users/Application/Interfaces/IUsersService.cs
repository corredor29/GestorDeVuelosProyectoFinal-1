using GestorDeVuelosProyectoFinal.src.Moduls.Users.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Users.Application.Interfaces;

public interface IUsersService
{
    Task<User> CreateAsync(int id, string username, string password, int roleId, int? personId = null, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetInactiveUsersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<User> UpdateAsync(int id, string? newPassword, int? newRoleId, CancellationToken cancellationToken = default);
    Task<User> ToggleActiveAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}