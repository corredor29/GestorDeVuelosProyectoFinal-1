namespace GestorDeVuelosProyectoFinal.src.Shared.Contracts;

// Contrato mínimo para confirmar o descartar cambios sobre la unidad de trabajo actual.
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task RollbackAsync();
}
