using GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Domain.Aggregate;

namespace GestorDeVuelosProyectoFinal.src.Moduls.Permissions.Application.Interfaces;

public interface IPermissionsService
{
    Task<Permission> CreateAsync(int id, string name, string? description, CancellationToken cancellationToken = default);
    Task<Permission?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Permission>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Permission> UpdateAsync(int id, string? newName, string? newDescription, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}