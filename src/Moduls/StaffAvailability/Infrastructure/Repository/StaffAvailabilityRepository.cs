using GestorDeVuelosProyectoFinal.Moduls.Personal.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.AvailabilityStates.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Aggregate;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.Repositories;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Domain.ValueObject;
using GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Infrastructure.Entities;
using GestorDeVuelosProyectoFinal.src.Shared.Context;
using Microsoft.EntityFrameworkCore;

namespace GestorDeVuelosProyectoFinal.Moduls.StaffAvailability.Infrastructure.Repository;

public sealed class StaffAvailabilityRepository : IStaffAvailabilityRepository
{
    private readonly AppDbContext _context;

    public StaffAvailabilityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<StaffAvailabilityRecord?> GetByIdAsync(StaffAvailabilityId id)
    {
        var entity = await _context.StaffAvailabilities.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id.Value);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<IEnumerable<StaffAvailabilityRecord>> GetByStaffIdAsync(PersonalId staffId)
    {
        var entities = await _context.StaffAvailabilities.AsNoTracking()
            .Where(x => x.StaffId == staffId.Value)
            .OrderBy(x => x.StartsAt)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<StaffAvailabilityRecord>> GetByDateRangeAsync(DateTime startsAt, DateTime endsAt)
    {
        EnsureValidRange(startsAt, endsAt);

        var entities = await _context.StaffAvailabilities.AsNoTracking()
            .Where(x => x.StartsAt < endsAt && x.EndsAt > startsAt)
            .OrderBy(x => x.StartsAt)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<IEnumerable<StaffAvailabilityRecord>> GetAllAsync()
    {
        var entities = await _context.StaffAvailabilities.AsNoTracking()
            .OrderBy(x => x.Id)
            .ToListAsync();

        return entities.Select(MapToDomain);
    }

    public async Task<bool> HasBlockingAvailabilityAsync(PersonalId staffId, DateTime startsAt, DateTime endsAt)
    {
        EnsureValidRange(startsAt, endsAt);

        return await (
            from availability in _context.StaffAvailabilities.AsNoTracking()
            join status in _context.AvailabilityStates.AsNoTracking() on availability.AvailabilityStatusId equals status.Id
            where availability.StaffId == staffId.Value
                && availability.StartsAt < endsAt
                && availability.EndsAt > startsAt
                && status.Name.ToLower() != "available"
            select availability.Id
        ).AnyAsync();
    }

    public async Task SaveAsync(StaffAvailabilityRecord availability)
    {
        await _context.StaffAvailabilities.AddAsync(MapToEntity(availability));
    }

    public Task UpdateAsync(StaffAvailabilityRecord availability)
    {
        _context.StaffAvailabilities.Update(MapToEntity(availability));
        return Task.CompletedTask;
    }

    public async Task DeleteByIdAsync(StaffAvailabilityId id)
    {
        var entity = await _context.StaffAvailabilities.FirstOrDefaultAsync(x => x.Id == id.Value);
        if (entity is not null)
            _context.StaffAvailabilities.Remove(entity);
    }

    public async Task<int> DeleteByStaffIdAsync(PersonalId staffId)
    {
        var entities = await _context.StaffAvailabilities
            .Where(x => x.StaffId == staffId.Value)
            .ToListAsync();

        if (entities.Count > 0)
            _context.StaffAvailabilities.RemoveRange(entities);

        return entities.Count;
    }

    public Task<int> DeleteExactBlockAsync(PersonalId staffId, AvailabilityStatesId stateId, DateTime startsAt, DateTime endsAt, string? notes)
    {
        return _context.StaffAvailabilities
            .Where(x =>
                x.StaffId == staffId.Value &&
                x.AvailabilityStatusId == stateId.Value &&
                x.StartsAt == startsAt &&
                x.EndsAt == endsAt &&
                x.Notes == notes)
            .ExecuteDeleteAsync();
    }

    public Task<bool> HasBlockingAvailabilityExcludingExactBlockAsync(
        PersonalId staffId,
        DateTime startsAt,
        DateTime endsAt,
        AvailabilityStatesId excludedStateId,
        DateTime excludedStartsAt,
        DateTime excludedEndsAt,
        string? excludedNotes)
    {
        EnsureValidRange(startsAt, endsAt);

        return (
            from availability in _context.StaffAvailabilities.AsNoTracking()
            join status in _context.AvailabilityStates.AsNoTracking() on availability.AvailabilityStatusId equals status.Id
            where availability.StaffId == staffId.Value
                && availability.StartsAt < endsAt
                && availability.EndsAt > startsAt
                && status.Name.ToLower() != "available"
                && !(availability.AvailabilityStatusId == excludedStateId.Value
                    && availability.StartsAt == excludedStartsAt
                    && availability.EndsAt == excludedEndsAt
                    && availability.Notes == excludedNotes)
            select availability.Id
        ).AnyAsync();
    }

    private static StaffAvailabilityRecord MapToDomain(StaffAvailabilityEntity entity)
    {
        return StaffAvailabilityRecord.FromPrimitives(
            entity.Id,
            entity.StaffId,
            entity.AvailabilityStatusId,
            entity.StartsAt,
            entity.EndsAt,
            entity.Notes);
    }

    private static StaffAvailabilityEntity MapToEntity(StaffAvailabilityRecord aggregate)
    {
        return new StaffAvailabilityEntity
        {
            Id = aggregate.Id?.Value ?? 0,
            StaffId = aggregate.StaffId.Value,
            AvailabilityStatusId = aggregate.StateId.Value,
            StartsAt = aggregate.Dates.StartDate,
            EndsAt = aggregate.Dates.EndDate,
            Notes = aggregate.Observation.Value
        };
    }

    private static void EnsureValidRange(DateTime startsAt, DateTime endsAt)
    {
        if (endsAt <= startsAt)
            throw new ArgumentException("La fecha final debe ser mayor que la fecha inicial.");
    }
}
