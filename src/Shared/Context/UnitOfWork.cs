using GestorDeVuelosProyectoFinal.src.Shared.Contracts;
namespace GestorDeVuelosProyectoFinal.src.Shared.Context;

// Esta implementación es mínima, pero sirve para centralizar el SaveChanges
// y el rollback lógico del ChangeTracker.
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RollbackAsync()
    {
        // Como EF ya tiene entidades rastreadas, limpiar el tracker es suficiente
        // para descartar cambios en memoria que todavía no se guardaron.
        _context.ChangeTracker.Clear();
    }
}
