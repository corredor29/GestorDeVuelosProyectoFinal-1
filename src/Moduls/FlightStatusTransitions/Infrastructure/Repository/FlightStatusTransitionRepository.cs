using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Domain.Repositories;
using GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.src.Moduls.FlightStatusTransitions.Infrastructure.Repository;

public sealed class FlightStatusTransitionRepository : IFlightStatusTransitionRepository
{
    private readonly AppDbContext _context;

    public FlightStatusTransitionRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<bool> IsTransitionAllowedAsync(int fromStatusId, int toStatusId, CancellationToken cancellationToken = default)
        => _context.FlightStatusTransitions.AsNoTracking()
            .AnyAsync(
                x => x.FromStatusId == fromStatusId && x.ToStatusId == toStatusId,
                cancellationToken);

    public async Task<IReadOnlyList<int>> GetAllowedToStatusIdsAsync(int fromStatusId, CancellationToken cancellationToken = default)
    {
        var list = await _context.FlightStatusTransitions.AsNoTracking()
            .Where(x => x.FromStatusId == fromStatusId)
            .OrderBy(x => x.ToStatusId)
            .Select(x => x.ToStatusId)
            .ToListAsync(cancellationToken);
        return list;
    }
}
