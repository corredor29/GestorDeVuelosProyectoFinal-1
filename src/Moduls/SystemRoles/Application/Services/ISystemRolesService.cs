using GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.SystemRoles.Application.Interfaces;

public interface ISystemRolesService
{
    Task<SystemRole> CreateAsync(int id, string name, string? description, CancellationToken cancellationToken = default);
    Task<SystemRole?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<SystemRole>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SystemRole> UpdateAsync(int id, string? newName, string? newDescription, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}