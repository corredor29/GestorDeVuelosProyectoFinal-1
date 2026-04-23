using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Infrastructure.Entity;
using GestorDeVuelosProyectoFinal.Moduls.FlightRoles.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Moduls.Flights.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.FlightAssignments.Infrastructure.Repository;

public sealed class FlightAssignmentsRepository : IFlightAssignmentsRepository
{
    private readonly AppDbContext _context;

    public FlightAssignmentsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FlightAssignment?> GetByIdAsync(FlightAssignmentId id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.FlightAssignments.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<FlightAssignment?> GetByFlightAndStaffAsync(FlightsId flightId, PersonalId staffId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.FlightAssignments.AsNoTracking()
            .FirstOrDefaultAsync(x => x.FlightId == flightId.Value && x.StaffId == staffId.Value, cancellationToken);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<FlightAssignment>> GetByFlightIdAsync(FlightsId flightId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.FlightAssignments.AsNoTracking()
            .Where(x => x.FlightId == flightId.Value)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<FlightAssignment>> GetByStaffIdAsync(PersonalId staffId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.FlightAssignments.AsNoTracking()
            .Where(x => x.StaffId == staffId.Value)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<FlightAssignment>> GetByRoleIdAsync(FlightRolesId flightRoleId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.FlightAssignments.AsNoTracking()
            .Where(x => x.FlightRoleId == flightRoleId.Value)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<FlightAssignment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _context.FlightAssignments.AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        return entities.Select(MapToDomain);
    }

    public async Task SaveAsync(FlightAssignment assignment, CancellationToken cancellationToken = default)
    {
        await _context.FlightAssignments.AddAsync(new FlightAssignmentEntity
        {
            FlightId = assignment.FlightId.Value,
            StaffId = assignment.StaffId.Value,
            FlightRoleId = assignment.FlightRoleId.Value
        }, cancellationToken);
    }

    public async Task UpdateAsync(FlightAssignment assignment, CancellationToken cancellationToken = default)
    {
        if (assignment.Id is null)
            throw new InvalidOperationException("No se puede actualizar una asignacion de tripulacion sin id.");

        var entity = await _context.FlightAssignments
            .FirstOrDefaultAsync(x => x.Id == assignment.Id.Value, cancellationToken);

        if (entity is null)
            throw new InvalidOperationException($"No se encontro la asignacion de tripulacion con id {assignment.Id.Value}.");

        entity.FlightId = assignment.FlightId.Value;
        entity.StaffId = assignment.StaffId.Value;
        entity.FlightRoleId = assignment.FlightRoleId.Value;
    }

    public async Task DeleteAsync(FlightAssignmentId id, CancellationToken cancellationToken = default)
    {
        await _context.FlightAssignments
            .Where(x => x.Id == id.Value)
            .ExecuteDeleteAsync(cancellationToken);
    }

    private static FlightAssignment MapToDomain(FlightAssignmentEntity entity)
        => FlightAssignment.FromPrimitives(entity.Id, entity.FlightId, entity.StaffId, entity.FlightRoleId);
}
